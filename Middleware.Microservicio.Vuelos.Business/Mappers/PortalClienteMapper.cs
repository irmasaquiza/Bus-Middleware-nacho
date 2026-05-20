using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Portal;
using Middleware.Vuelos.DataManagement.Models.Reservas;
using Middleware.Vuelos.DataManagement.Models.Vuelos;

namespace Middleware.Vuelos.Business.Mappers;

// ============================================================
// PortalClienteMapper
// ============================================================
using Middleware.Vuelos.Business.DTOs.Portal;
using Middleware.Vuelos.DataManagement.Models.Reservas;
using Middleware.Vuelos.DataManagement.Models.Vuelos;

public static class PortalClienteMapper
{
    public static ReservaPortalResponse ToReservaPortalResponse(
        ReservaDataModel reserva,
        VueloDataModel? vuelo) => new()
        {
            IdReserva = reserva.IdReserva,
            CodigoReserva = reserva.CodigoReserva,
            IdVuelo = reserva.IdVuelo,
            NumeroVuelo = vuelo?.NumeroVuelo ?? string.Empty,
            FechaReservaUtc = reserva.FechaReservaUtc,
            TotalReserva = reserva.TotalReserva,
            EstadoReserva = reserva.EstadoReserva,
            ContactoEmail = reserva.ContactoEmail,
            Detalles = reserva.Detalles.Select(d => new ReservaDetallePortalResponse
            {
                IdDetalle = d.IdDetalle,
                IdPasajero = d.IdPasajero,
                IdAsiento = d.IdAsiento,
                NumeroAsiento = string.Empty, // enriquecido si se necesita
                Clase = string.Empty,
                TotalLinea = d.TotalLinea,
                EstadoDetalle = d.EstadoDetalle
            }).ToList()
        };

    public static FacturaPortalResponse ToFacturaPortalResponse(
        FacturaDataModel factura,
        string codigoReserva) => new()
        {
            IdFactura = factura.IdFactura,
            NumeroFactura = factura.NumeroFactura,
            CodigoReserva = codigoReserva,
            FechaEmision = factura.FechaEmision,
            Subtotal = factura.Subtotal,
            ValorIva = factura.ValorIva,
            Total = factura.Total,
            EstadoFactura = factura.EstadoFactura
        };

    public static BoletoPortalResponse ToBoletoPortalResponse(
        BoletoDataModel boleto,
        VueloDataModel? vuelo,
        List<AsientoDataModel> asientos,
        string codigoReserva)
    {
        var asiento = asientos?.FirstOrDefault(a => a.IdAsiento == boleto.IdAsiento);

        return new BoletoPortalResponse
        {
            IdBoleto = boleto.IdBoleto,
            CodigoBoleto = boleto.CodigoBoleto,
            IdVuelo = boleto.IdVuelo,
            NumeroVuelo = vuelo?.NumeroVuelo ?? string.Empty,
            NumeroAsiento = asiento?.NumeroAsiento ?? string.Empty,
            Clase = boleto.Clase,
            PrecioFinal = boleto.PrecioFinal,
            EstadoBoleto = boleto.EstadoBoleto,
            FechaEmision = boleto.FechaEmision,
            CodigoReserva = codigoReserva
        };
    }
}
