using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Portal;
using Middleware.Vuelos.DataManagement.Models.Reservas;
using Middleware.Vuelos.DataManagement.Models.Vuelos;

namespace Middleware.Vuelos.Business.Mappers;

// ============================================================
// VuelosBusinessMapper
// ============================================================
public static class VuelosBusinessMapper
{
    public static VueloResponse ToVueloResponse(
        VueloDataModel vuelo,
        List<EscalaDataModel> escalas) => new()
        {
            IdVuelo = vuelo.IdVuelo,
            IdAeropuertoOrigen = vuelo.IdAeropuertoOrigen,
            IdAeropuertoDestino = vuelo.IdAeropuertoDestino,
            NumeroVuelo = vuelo.NumeroVuelo,
            FechaHoraSalida = vuelo.FechaHoraSalida,
            FechaHoraLlegada = vuelo.FechaHoraLlegada,
            DuracionMin = vuelo.DuracionMin,
            PrecioBase = vuelo.PrecioBase,
            CapacidadTotal = vuelo.CapacidadTotal,
            EstadoVuelo = vuelo.EstadoVuelo,
            Escalas = escalas.Select(ToEscalaResponse).ToList()
        };

    public static EscalaResponse ToEscalaResponse(EscalaDataModel model) => new()
    {
        IdEscala = model.IdEscala,
        IdVuelo = model.IdVuelo,
        IdAeropuerto = model.IdAeropuerto,
        Orden = model.Orden,
        FechaHoraLlegada = model.FechaHoraLlegada,
        FechaHoraSalida = model.FechaHoraSalida,
        DuracionMin = model.DuracionMin,
        TipoEscala = model.TipoEscala,
        Terminal = model.Terminal,
        Puerta = model.Puerta
    };

    public static AsientoResponse ToAsientoResponse(AsientoDataModel model) => new()
    {
        IdAsiento = model.IdAsiento,
        IdVuelo = model.IdVuelo,
        NumeroAsiento = model.NumeroAsiento,
        Clase = model.Clase,
        Disponible = model.Disponible,
        PrecioExtra = model.PrecioExtra,
        Posicion = model.Posicion,
        Estado = model.Estado
    };
}