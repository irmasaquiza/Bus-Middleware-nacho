using Microsoft.Extensions.Logging;
using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.Exceptions;
using Middleware.Vuelos.Business.Interfaces;
using Middleware.Vuelos.Business.Mappers;
using Middleware.Vuelos.DataAccess.Models;
using Middleware.Vuelos.DataManagement.Interfaces;

namespace Middleware.Vuelos.Business.Orchestrators;

/// <summary>
/// Orquestador de reservas — el más crítico del Bus.
///
/// Coordina MS Vuelos, MS Clientes y MS ReservasF para el flujo completo:
/// crear reserva → pagar → bloquear asientos.
///
/// Maneja compensaciones automáticas si algo falla después
/// de que se creó la reserva (patrón Saga simplificado).
/// </summary>
public class ReservaOrchestrator : IReservaOrchestrator
{
    private readonly IReservasDataService _reservasDataService;
    private readonly IVuelosDataService _vuelosDataService;
    private readonly IClientesDataService _clientesDataService;
    private readonly ILogger<ReservaOrchestrator> _logger;

    public ReservaOrchestrator(
        IReservasDataService reservasDataService,
        IVuelosDataService vuelosDataService,
        IClientesDataService clientesDataService,
        ILogger<ReservaOrchestrator> logger)
    {
        _reservasDataService = reservasDataService;
        _vuelosDataService = vuelosDataService;
        _clientesDataService = clientesDataService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ReservaResponse> CrearReservaAsync(
        CrearReservaRequest request,
        int idCliente,
        string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][ReservaOrchestrator] CrearReserva iniciado. " +
            "IdCliente={IdCliente} IdVuelo={IdVuelo} Pasajeros={Pasajeros}",
            idCliente, request.IdVuelo, request.Detalles.Count);

        // ── 1. Validar que el vuelo existe y está operable ───────────────────────
        var vuelo = await _vuelosDataService.ValidarVueloOperableAsync(request.IdVuelo)
            ?? throw new BusinessException(
                $"El vuelo {request.IdVuelo} no existe o no está disponible para reservas.");

        // ── 2. Validar que el cliente existe y está activo ───────────────────────
        var cliente = await _clientesDataService.ValidarClienteActivoAsync(
            idCliente, jwtToken)
            ?? throw new BusinessException(
                $"El cliente {idCliente} no existe o no está activo.");

        // ── 3. Validar que cada asiento existe y está disponible ─────────────────
        foreach (var detalle in request.Detalles)
        {
            var asiento = await _vuelosDataService.ValidarAsientoDisponibleAsync(
                request.IdVuelo, detalle.IdAsiento);
            if (asiento is null)
                throw new BusinessException(
                    $"El asiento {detalle.IdAsiento} no está disponible para el vuelo {request.IdVuelo}.");

            var pasajero = await _clientesDataService.ValidarPasajeroActivoAsync(
                detalle.IdPasajero, jwtToken);
            if (pasajero is null)
                throw new BusinessException(
                    $"El pasajero {detalle.IdPasajero} no existe o no está activo.");
        }

        // ── 4. Crear la reserva en MS ReservasF ──────────────────────────────────
        var detalles = request.Detalles
            .Select(d => (d.IdPasajero, d.IdAsiento))
            .ToList();

        var reserva = await _reservasDataService.CrearReservaAsync(
            idCliente: idCliente,
            idVuelo: request.IdVuelo,
            fechaInicio: vuelo.FechaHoraSalida,
            fechaFin: vuelo.FechaHoraLlegada,
            subtotalReserva: request.SubtotalReserva,   // ← agregar
            valorIva: request.ValorIva,           // ← agregar
            totalReserva: request.TotalReserva,       // ← agregar
            contactoEmail: request.ContactoEmail,
            contactoTelefono: request.ContactoTelefono,
            observaciones: request.Observaciones,
            detalles: request.Detalles
                .Select(d => (
                    d.IdPasajero,
                    d.IdAsiento,
                    d.SubtotalLinea,   // ← agregar
                    d.ValorIvaLinea,   // ← agregar
                    d.TotalLinea       // ← agregar
                )).ToList(),
            jwtToken: jwtToken)
            ?? throw new BusinessException("No se pudo crear la reserva. Intente nuevamente.");

        _logger.LogInformation(
            "[Bus][ReservaOrchestrator] Reserva creada. " +
            "IdReserva={IdReserva} CodigoReserva={CodigoReserva}",
            reserva.IdReserva, reserva.CodigoReserva);

        return ReservasBusinessMapper.ToReservaResponse(reserva);
    }

    /// <inheritdoc />
    public async Task<ReservaResponse> PagarReservaAsync(
        int idReserva,
        PagarReservaRequest request,
        string jwtToken)
    {
        var reserva = await _reservasDataService.ValidarReservaPagableAsync(
            idReserva, jwtToken)
            ?? throw new BusinessException(
                $"La reserva {idReserva} no existe o no está en estado pagable.");

        var pagarDto = new PagarReservaRequestDto
        {
            CargoServicio = request.CargoServicio,
            Equipaje = request.Equipaje.Select(e => new PagarReservaEquipajeDto
            {
                IdDetalle = e.IdDetalle,
                Tipo = e.Tipo,
                PesoKg = e.PesoKg,
                DescripcionEquipaje = e.DescripcionEquipaje
            }).ToList()
        };

        var reservaPagada = await _reservasDataService.PagarReservaAsync(
            idReserva, pagarDto, jwtToken);

        if (reservaPagada is null)
            throw new BusinessException(
                "No se pudo procesar el pago de la reserva. Intente nuevamente.");

        return ReservasBusinessMapper.ToReservaResponse(reservaPagada);
    }

    /// <inheritdoc />
    public async Task<ReservaResponse> CancelarReservaAsync(
        int idReserva,
        CancelarReservaRequest request,
        string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][ReservaOrchestrator] CancelarReserva. IdReserva={IdReserva}",
            idReserva);

        // Obtener la reserva para saber qué asientos liberar
        var reserva = await _reservasDataService.GetReservaByIdAsync(
            idReserva, jwtToken)
            ?? throw new NotFoundException(
                $"La reserva {idReserva} no existe.");

        // Cancelar en MS ReservasF
        var reservaCancelada = await _reservasDataService.CancelarReservaAsync(
            idReserva, request.Motivo, jwtToken)
            ?? throw new BusinessException("No se pudo cancelar la reserva.");

        // Si la reserva estaba emitida (pagada), liberar los asientos en MS Vuelos
        if (reserva.EsEmitida)
        {
            foreach (var detalle in reserva.Detalles)
            {
                await _vuelosDataService.LiberarAsientoAsync(
                    reserva.IdVuelo, detalle.IdAsiento, jwtToken);
            }

            _logger.LogInformation(
                "[Bus][ReservaOrchestrator] Asientos liberados tras cancelación. " +
                "IdReserva={IdReserva}", idReserva);
        }

        return ReservasBusinessMapper.ToReservaResponse(reservaCancelada);
    }

    /// <inheritdoc />
    public async Task<ReservaResponse> GetReservaByIdAsync(
        int idReserva,
        string jwtToken)
    {
        var reserva = await _reservasDataService.GetReservaByIdAsync(
            idReserva, jwtToken)
            ?? throw new NotFoundException($"La reserva {idReserva} no existe.");

        return ReservasBusinessMapper.ToReservaResponse(reserva);
    }

    /// <inheritdoc />
    public async Task<List<BoletoResponse>> GetBoletosByReservaAsync(
        int idReserva,
        string jwtToken)
    {
        var boletos = await _reservasDataService.GetBoletosByReservaAsync(
            idReserva, jwtToken);

        return boletos.Select(ReservasBusinessMapper.ToBoletoResponse).ToList();
    }
}