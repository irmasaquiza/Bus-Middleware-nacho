using Microsoft.Extensions.Logging;
using Middleware.Vuelos.Business.Exceptions;

using Middleware.Vuelos.Business.DTOs.Portal;
using Middleware.Vuelos.Business.Exceptions;
using Middleware.Vuelos.Business.Interfaces;
using Middleware.Vuelos.Business.Mappers;
using Middleware.Vuelos.DataManagement.Interfaces;

namespace Middleware.Vuelos.Business.Orchestrators;

/// <summary>
/// Orquestador del Portal del Cliente.
/// Contiene los endpoints migrados desde MS Clientes al Bus.
/// Todos los métodos filtran por id_cliente extraído del JWT.
/// El cliente solo puede ver SUS propios datos.
/// </summary>
public class PortalClienteOrchestrator : IPortalClienteOrchestrator
{
    private readonly IReservasDataService _reservasDataService;
    private readonly IVuelosDataService _vuelosDataService;
    private readonly ILogger<PortalClienteOrchestrator> _logger;

    public PortalClienteOrchestrator(
        IReservasDataService reservasDataService,
        IVuelosDataService vuelosDataService,
        ILogger<PortalClienteOrchestrator> logger)
    {
        _reservasDataService = reservasDataService;
        _vuelosDataService = vuelosDataService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<List<ReservaPortalResponse>> GetMisReservasAsync(
        int idCliente, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][PortalClienteOrchestrator] GetMisReservas. IdCliente={IdCliente}",
            idCliente);

        var reservas = await _reservasDataService.GetReservasByClienteAsync(
            idCliente, jwtToken);

        return reservas.Select(r =>
            PortalClienteMapper.ToReservaPortalResponse(r, null)).ToList();
    }

    /// <inheritdoc />
    public async Task<ReservaPortalResponse> GetDetalleReservaAsync(
        int idReserva, int idCliente, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][PortalClienteOrchestrator] GetDetalleReserva. " +
            "IdReserva={IdReserva} IdCliente={IdCliente}",
            idReserva, idCliente);

        var reserva = await _reservasDataService.GetReservaByIdAsync(
            idReserva, jwtToken)
            ?? throw new NotFoundException($"La reserva {idReserva} no existe.");

        // Validar que la reserva pertenece al cliente autenticado
        if (reserva.IdCliente != idCliente)
            throw new UnauthorizedBusinessException(
                "No tienes permiso para ver esta reserva.");

        // Enriquecer con datos del vuelo
        var vuelo = await _vuelosDataService.GetVueloByIdAsync(reserva.IdVuelo);

        return PortalClienteMapper.ToReservaPortalResponse(reserva, vuelo);
    }

    /// <inheritdoc />
    public async Task<ReservaPortalResponse> GetReservaPorCodigoAsync(
        string codigoReserva, int idCliente, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][PortalClienteOrchestrator] GetReservaPorCodigo. " +
            "CodigoReserva={CodigoReserva} IdCliente={IdCliente}",
            codigoReserva, idCliente);

        var reserva = await _reservasDataService.GetReservaPorCodigoAsync(
            codigoReserva, jwtToken)
            ?? throw new NotFoundException(
                $"No se encontró reserva con código {codigoReserva}.");

        if (reserva.IdCliente != idCliente)
            throw new UnauthorizedBusinessException(
                "No tienes permiso para ver esta reserva.");

        var vuelo = await _vuelosDataService.GetVueloByIdAsync(reserva.IdVuelo);
        return PortalClienteMapper.ToReservaPortalResponse(reserva, vuelo);
    }

    /// <inheritdoc />
    public async Task<FacturaPortalResponse> GetFacturaDeReservaAsync(
        int idReserva, int idCliente, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][PortalClienteOrchestrator] GetFacturaDeReserva. " +
            "IdReserva={IdReserva} IdCliente={IdCliente}",
            idReserva, idCliente);

        // Validar que la reserva pertenece al cliente
        var reserva = await _reservasDataService.GetReservaByIdAsync(
            idReserva, jwtToken)
            ?? throw new NotFoundException($"La reserva {idReserva} no existe.");

        if (reserva.IdCliente != idCliente)
            throw new UnauthorizedBusinessException(
                "No tienes permiso para ver esta factura.");

        var factura = await _reservasDataService.GetFacturaByReservaAsync(
            idReserva, jwtToken)
            ?? throw new NotFoundException(
                $"No se encontró factura para la reserva {idReserva}.");

        return PortalClienteMapper.ToFacturaPortalResponse(factura, reserva.CodigoReserva);
    }

    /// <inheritdoc />
    public async Task<List<BoletoPortalResponse>> GetMisBoletosAsync(
        int idCliente, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][PortalClienteOrchestrator] GetMisBoletos. IdCliente={IdCliente}",
            idCliente);

        var boletos = await _reservasDataService.GetBoletosByClienteAsync(
            idCliente, jwtToken);

        return boletos.Select(b =>
            PortalClienteMapper.ToBoletoPortalResponse(b, null, [], null)).ToList();
    }

    /// <inheritdoc />
    public async Task<BoletoPortalResponse> GetBoletoDeReservaAsync(
        int idReserva, int idCliente, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][PortalClienteOrchestrator] GetBoletoDeReserva. " +
            "IdReserva={IdReserva} IdCliente={IdCliente}",
            idReserva, idCliente);

        // Validar que la reserva pertenece al cliente
        var reserva = await _reservasDataService.GetReservaByIdAsync(
            idReserva, jwtToken)
            ?? throw new NotFoundException($"La reserva {idReserva} no existe.");

        if (reserva.IdCliente != idCliente)
            throw new UnauthorizedBusinessException(
                "No tienes permiso para ver este boleto.");

        var boletos = await _reservasDataService.GetBoletosByReservaAsync(
            idReserva, jwtToken);

        if (!boletos.Any())
            throw new NotFoundException(
                $"No se encontraron boletos para la reserva {idReserva}.");

        // Enriquecer con datos del vuelo
        var vuelo = await _vuelosDataService.GetVueloByIdAsync(reserva.IdVuelo);
        var asientos = await _vuelosDataService.GetAsientosByVueloAsync(reserva.IdVuelo);

        return PortalClienteMapper.ToBoletoPortalResponse(
            boletos.First(), vuelo, asientos, reserva.CodigoReserva);
    }

    /// <inheritdoc />
    public async Task<List<FacturaPortalResponse>> GetMisFacturasAsync(
        int idCliente, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][PortalClienteOrchestrator] GetMisFacturas. IdCliente={IdCliente}",
            idCliente);

        var facturas = await _reservasDataService.GetFacturasByClienteAsync(
            idCliente, jwtToken);

        return facturas.Select(f =>
            PortalClienteMapper.ToFacturaPortalResponse(f, null)).ToList();
    }
}