using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Portal;
using Middleware.Vuelos.DataManagement.Models.Reservas;
using Middleware.Vuelos.DataManagement.Models.Vuelos;

namespace Middleware.Vuelos.Business.Mappers;

// ============================================================
// ReservasBusinessMapper
// ============================================================
public static class ReservasBusinessMapper
{
    public static ReservaResponse ToReservaResponse(ReservaDataModel model) => new()
    {
        IdReserva = model.IdReserva,
        GuidReserva = model.GuidReserva,
        CodigoReserva = model.CodigoReserva,
        IdCliente = model.IdCliente,
        IdVuelo = model.IdVuelo,
        FechaReservaUtc = model.FechaReservaUtc,
        SubtotalReserva = model.SubtotalReserva,
        ValorIva = model.ValorIva,
        TotalReserva = model.TotalReserva,
        EstadoReserva = model.EstadoReserva,
        ContactoEmail = model.ContactoEmail,
        ContactoTelefono = model.ContactoTelefono,
        Observaciones = model.Observaciones,
        Detalles = model.Detalles.Select(d => new ReservaDetalleResponse
        {
            IdDetalle = d.IdDetalle,
            IdPasajero = d.IdPasajero,
            IdAsiento = d.IdAsiento,
            SubtotalLinea = d.SubtotalLinea,
            TotalLinea = d.TotalLinea,
            EstadoDetalle = d.EstadoDetalle
        }).ToList()
    };

    public static BoletoResponse ToBoletoResponse(BoletoDataModel model) => new()
    {
        IdBoleto = model.IdBoleto,
        IdReserva = model.IdReserva,
        IdVuelo = model.IdVuelo,
        IdAsiento = model.IdAsiento,
        CodigoBoleto = model.CodigoBoleto,
        Clase = model.Clase,
        PrecioVueloBase = model.PrecioVueloBase,
        PrecioAsientoExtra = model.PrecioAsientoExtra,
        ImpuestosBoleto = model.ImpuestosBoleto,
        CargoEquipaje = model.CargoEquipaje,
        PrecioFinal = model.PrecioFinal,
        EstadoBoleto = model.EstadoBoleto,
        FechaEmision = model.FechaEmision
    };

    public static FacturaResponse ToFacturaResponse(FacturaDataModel model) => new()
    {
        IdFactura = model.IdFactura,
        GuidFactura = model.GuidFactura,
        IdCliente = model.IdCliente,
        IdReserva = model.IdReserva,
        NumeroFactura = model.NumeroFactura,
        FechaEmision = model.FechaEmision,
        Subtotal = model.Subtotal,
        ValorIva = model.ValorIva,
        CargoServicio = model.CargoServicio,
        Total = model.Total,
        EstadoFactura = model.EstadoFactura,
        ObservacionesFactura = model.ObservacionesFactura
    };
}