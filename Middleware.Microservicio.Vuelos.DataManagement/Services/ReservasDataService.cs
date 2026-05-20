using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Models.Reservas;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataManagement.Services;

/// <summary>
/// Implementación del servicio de datos de ReservasF en el Bus.
/// Llama al ReservasFClient (HTTP) y transforma DTOs en modelos internos.
/// </summary>
public class ReservasDataService : IReservasDataService
{
    private readonly IReservasFClient _reservasFClient;
    private readonly ILogger<ReservasDataService> _logger;

    public ReservasDataService(
        IReservasFClient reservasFClient,
        ILogger<ReservasDataService> logger)
    {
        _reservasFClient = reservasFClient;
        _logger = logger;
    }

    // ── RESERVAS ─────────────────────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<ReservaDataModel?> GetReservaByIdAsync(
        int idReserva, string jwtToken)
    {
        var dto = await _reservasFClient.GetReservaByIdAsync(idReserva, jwtToken);
        return dto is null ? null : MapReservaToModel(dto);
    }

    /// <inheritdoc />
    public async Task<ReservaDataModel?> CrearReservaAsync(
        int idCliente,
        int idVuelo,
        DateTime fechaInicio,
        DateTime fechaFin,
        decimal subtotalReserva,
        decimal valorIva,
        decimal totalReserva,
        string? contactoEmail,
        string? contactoTelefono,
        string? observaciones,
        List<(int idPasajero, int idAsiento, decimal subtotalLinea, decimal valorIvaLinea, decimal totalLinea)> detalles,
        string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][ReservasDataService] CrearReserva. " +
            "IdCliente={IdCliente} IdVuelo={IdVuelo} Detalles={Detalles}",
            idCliente, idVuelo, detalles.Count);

        var request = new CrearReservaRequestDto
        {
            IdCliente = idCliente,
            IdVuelo = idVuelo,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            SubtotalReserva = subtotalReserva,  // ← agregar
            ValorIva = valorIva,          // ← agregar
            TotalReserva = totalReserva,      // ← agregar
            OrigenCanalReserva = "WEB",
            ContactoEmail = contactoEmail,
            ContactoTelefono = contactoTelefono,
            Observaciones = observaciones,
            Detalles = detalles
                .Select(d => new CrearReservaDetalleDto
                {
                    IdPasajero = d.idPasajero,
                    IdAsiento = d.idAsiento, 
                    SubtotalLinea = d.subtotalLinea,  // ← agregar
                    ValorIvaLinea = d.valorIvaLinea,  // ← agregar
                    TotalLinea = d.totalLinea      // ← agregar
                })
                .ToList()
        };

        var dto = await _reservasFClient.CrearReservaAsync(request, jwtToken);
        return dto is null ? null : MapReservaToModel(dto);
    }

    /// <inheritdoc />
    public async Task<ReservaDataModel?> PagarReservaAsync(
        int idReserva,
        PagarReservaRequestDto request,
        string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][ReservasDataService] PagarReserva. IdReserva={IdReserva}",
            idReserva);

        var dto = await _reservasFClient.PagarReservaAsync(idReserva, request, jwtToken);

        if (dto is null)
        {
            _logger.LogError(
                "[Bus][ReservasDataService] PagarReserva devolvió null. " +
                "IdReserva={IdReserva}", idReserva);
            return null;
        }

        return MapReservaToModel(dto);
    }

    /// <inheritdoc />
    public async Task<ReservaDataModel?> CancelarReservaAsync(
        int idReserva, string motivo, string jwtToken)
    {
        _logger.LogWarning(
            "[Bus][ReservasDataService] CancelarReserva (compensación). " +
            "IdReserva={IdReserva} Motivo={Motivo}",
            idReserva, motivo);

        var dto = await _reservasFClient.CancelarReservaAsync(
            idReserva, motivo, jwtToken);

        return dto is null ? null : MapReservaToModel(dto);
    }

    /// <inheritdoc />
    public async Task<ReservaDataModel?> ValidarReservaPagableAsync(
        int idReserva, string jwtToken)
    {
        var reserva = await GetReservaByIdAsync(idReserva, jwtToken);

        if (reserva is null)
        {
            _logger.LogWarning(
                "[Bus][ReservasDataService] Reserva no encontrada. " +
                "IdReserva={IdReserva}", idReserva);
            return null;
        }

        if (!reserva.EsPagable)
        {
            _logger.LogWarning(
                "[Bus][ReservasDataService] Reserva no está en estado pagable. " +
                "IdReserva={IdReserva} EstadoReserva={EstadoReserva}",
                idReserva, reserva.EstadoReserva);
            return null;
        }

        return reserva;
    }

    // ── BOLETOS ──────────────────────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<BoletoDataModel?> GetBoletoByIdAsync(
        int idBoleto, string jwtToken)
    {
        var dto = await _reservasFClient.GetBoletoByIdAsync(idBoleto, jwtToken);
        return dto is null ? null : MapBoletoToModel(dto);
    }

    /// <inheritdoc />
    public async Task<List<BoletoDataModel>> GetBoletosByReservaAsync(
        int idReserva, string jwtToken)
    {
        var dtos = await _reservasFClient.GetBoletosByReservaAsync(idReserva, jwtToken);
        return dtos.Select(MapBoletoToModel).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReservaDataModel>> GetReservasByClienteAsync(
        int idCliente, string jwtToken)
    {
        var paged = await _reservasFClient.GetReservasPagedAsync(
            idCliente: idCliente,
            idVuelo: null,
            codigoReserva: null,
            estadoReserva: null,
            page: 1,
            pageSize: 100,
            jwtToken: jwtToken);

        return paged?.Items?.Select(MapReservaToModel).ToList() ?? [];
    }

    /// <inheritdoc />
    public async Task<ReservaDataModel?> GetReservaPorCodigoAsync(
        string codigoReserva, string jwtToken)
    {
        var paged = await _reservasFClient.GetReservasPagedAsync(
            idCliente: null,
            idVuelo: null,
            codigoReserva: codigoReserva,
            estadoReserva: null,
            page: 1,
            pageSize: 1,
            jwtToken: jwtToken);

        var dto = paged?.Items?.FirstOrDefault();
        return dto is null ? null : MapReservaToModel(dto);
    }

    /// <inheritdoc />
    public async Task<List<BoletoDataModel>> GetBoletosByClienteAsync(
        int idCliente, string jwtToken)
    {
        var paged = await _reservasFClient.GetBoletosPagedAsync(
            idReserva: null,
            idVuelo: null,
            codigoBoleto: null,
            estadoBoleto: null,
            page: 1,
            pageSize: 100,
            jwtToken: jwtToken);

        return paged?.Items?.Select(MapBoletoToModel).ToList() ?? [];
    }

    /// <inheritdoc />
    public async Task<List<FacturaDataModel>> GetFacturasByClienteAsync(
        int idCliente, string jwtToken)
    {
        var paged = await _reservasFClient.GetFacturasPagedAsync(
            idCliente: idCliente,
            idReserva: null,
            numeroFactura: null,
            estadoFactura: null,
            page: 1,
            pageSize: 100,
            jwtToken: jwtToken);

        return paged?.Items?.Select(MapFacturaToModel).ToList() ?? [];
    }

    // ── FACTURAS ─────────────────────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<FacturaDataModel?> GetFacturaByReservaAsync(
        int idReserva, string jwtToken)
    {
        var dto = await _reservasFClient
            .GetFacturaByReservaAsync(idReserva, jwtToken);
        return dto is null ? null : MapFacturaToModel(dto);
    }

    // ── EQUIPAJE ─────────────────────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<List<EquipajeDataModel>> GetEquipajeByBoletoAsync(
        int idBoleto, string jwtToken)
    {
        var dtos = await _reservasFClient.GetEquipajeByBoletoAsync(idBoleto, jwtToken);
        return dtos.Select(MapEquipajeToModel).ToList();
    }

    /// <inheritdoc />
    public async Task<EquipajeDataModel?> AgregarEquipajeAsync(
        int idBoleto,
        string tipo,
        decimal pesoKg,
        string? descripcion,
        string? dimensionesCm,
        string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][ReservasDataService] AgregarEquipaje. " +
            "IdBoleto={IdBoleto} Tipo={Tipo} PesoKg={PesoKg}",
            idBoleto, tipo, pesoKg);

        var dto = await _reservasFClient.AgregarEquipajeAsync(
            idBoleto,
            new AgregarEquipajeRequestDto
            {
                Tipo = tipo,
                PesoKg = pesoKg,
                DescripcionEquipaje = descripcion,
                DimensionesCm = dimensionesCm
            },
            jwtToken);

        return dto is null ? null : MapEquipajeToModel(dto);
    }

    // ── Mappers privados ─────────────────────────────────────────────────────────

    private static ReservaDataModel MapReservaToModel(ReservaDto dto) => new()
    {
        IdReserva = dto.IdReserva,
        GuidReserva = dto.GuidReserva,
        CodigoReserva = dto.CodigoReserva,
        IdCliente = dto.IdCliente,
        IdVuelo = dto.IdVuelo,
        FechaReservaUtc = dto.FechaReservaUtc,
        FechaInicio = dto.FechaInicio,
        FechaFin = dto.FechaFin,
        SubtotalReserva = dto.SubtotalReserva,
        ValorIva = dto.ValorIva,
        TotalReserva = dto.TotalReserva,
        OrigenCanalReserva = dto.OrigenCanalReserva,
        EstadoReserva = dto.EstadoReserva,
        FechaConfirmacionUtc = dto.FechaConfirmacionUtc,
        FechaCancelacionUtc = dto.FechaCancelacionUtc,
        MotivoCancelacion = dto.MotivoCancelacion,
        ContactoEmail = dto.ContactoEmail,
        ContactoTelefono = dto.ContactoTelefono,
        Observaciones = dto.Observaciones,
        EsEliminado = dto.EsEliminado,
        Detalles = dto.Detalles.Select(d => new ReservaDetalleDataModel
        {
            IdDetalle = d.IdDetalle,
            IdReserva = d.IdReserva,
            IdPasajero = d.IdPasajero,
            IdAsiento = d.IdAsiento,
            SubtotalLinea = d.SubtotalLinea,
            ValorIvaLinea = d.ValorIvaLinea,
            TotalLinea = d.TotalLinea,
            EstadoDetalle = d.EstadoDetalle,
            EsEliminado = d.EsEliminado
        }).ToList()
    };

    private static BoletoDataModel MapBoletoToModel(BoletoDto dto) => new()
    {
        IdBoleto = dto.IdBoleto,
        IdReserva = dto.IdReserva,
        IdDetalle = dto.IdDetalle,
        IdVuelo = dto.IdVuelo,
        IdAsiento = dto.IdAsiento,
        IdFactura = dto.IdFactura,
        CodigoBoleto = dto.CodigoBoleto,
        Clase = dto.Clase,
        PrecioVueloBase = dto.PrecioVueloBase,
        PrecioAsientoExtra = dto.PrecioAsientoExtra,
        ImpuestosBoleto = dto.ImpuestosBoleto,
        CargoEquipaje = dto.CargoEquipaje,
        PrecioFinal = dto.PrecioFinal,
        EstadoBoleto = dto.EstadoBoleto,
        FechaEmision = dto.FechaEmision,
        EsEliminado = dto.EsEliminado
    };

    private static FacturaDataModel MapFacturaToModel(FacturaDto dto) => new()
    {
        IdFactura = dto.IdFactura,
        GuidFactura = dto.GuidFactura,
        IdCliente = dto.IdCliente,
        IdReserva = dto.IdReserva,
        NumeroFactura = dto.NumeroFactura,
        FechaEmision = dto.FechaEmision,
        Subtotal = dto.Subtotal,
        ValorIva = dto.ValorIva,
        CargoServicio = dto.CargoServicio,
        Total = dto.Total,
        EstadoFactura = dto.EstadoFactura,
        ObservacionesFactura = dto.ObservacionesFactura,
        EsEliminado = dto.EsEliminado
    };

    private static EquipajeDataModel MapEquipajeToModel(EquipajeDto dto) => new()
    {
        IdEquipaje = dto.IdEquipaje,
        IdBoleto = dto.IdBoleto,
        Tipo = dto.Tipo,
        PesoKg = dto.PesoKg,
        DescripcionEquipaje = dto.DescripcionEquipaje,
        PrecioExtra = dto.PrecioExtra,
        DimensionesCm = dto.DimensionesCm,
        NumeroEtiqueta = dto.NumeroEtiqueta,
        EstadoEquipaje = dto.EstadoEquipaje,
        EsEliminado = dto.EsEliminado
    };


}