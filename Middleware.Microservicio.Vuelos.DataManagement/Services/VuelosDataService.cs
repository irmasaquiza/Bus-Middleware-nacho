using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Models.Vuelos;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataManagement.Services;

/// <summary>
/// Implementación del servicio de datos de Vuelos en el Bus.
/// Llama al VuelosClient (HTTP) y transforma DTOs en modelos internos.
/// </summary>
public class VuelosDataService : IVuelosDataService
{
    private readonly IVuelosClient _vuelosClient;
    private readonly ILogger<VuelosDataService> _logger;

    public VuelosDataService(
        IVuelosClient vuelosClient,
        ILogger<VuelosDataService> logger)
    {
        _vuelosClient = vuelosClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<VueloDataModel?> GetVueloByIdAsync(int idVuelo)
    {
        var dto = await _vuelosClient.GetVueloByIdAsync(idVuelo);
        return dto is null ? null : MapVueloToModel(dto);
    }

    /// <inheritdoc />
    public async Task<VueloDataModel?> ValidarVueloOperableAsync(int idVuelo)
    {
        var vuelo = await GetVueloByIdAsync(idVuelo);

        if (vuelo is null)
        {
            _logger.LogWarning(
                "[Bus][VuelosDataService] Vuelo no encontrado. IdVuelo={IdVuelo}",
                idVuelo);
            return null;
        }

        if (!vuelo.EsOperable)
        {
            _logger.LogWarning(
                "[Bus][VuelosDataService] Vuelo no operable. " +
                "IdVuelo={IdVuelo} EstadoVuelo={EstadoVuelo} Estado={Estado}",
                idVuelo, vuelo.EstadoVuelo, vuelo.Estado);
            return null;
        }

        return vuelo;
    }

    /// <inheritdoc />
    public async Task<List<AsientoDataModel>> GetAsientosByVueloAsync(int idVuelo)
    {
        var dtos = await _vuelosClient.GetAsientosByVueloAsync(idVuelo);
        return dtos.Select(MapAsientoToModel).ToList();
    }

    /// <inheritdoc />
    public async Task<AsientoDataModel?> GetAsientoByIdAsync(int idVuelo, int idAsiento)
    {
        var dto = await _vuelosClient.GetAsientoByIdAsync(idVuelo, idAsiento);
        return dto is null ? null : MapAsientoToModel(dto);
    }

    /// <inheritdoc />
    public async Task<AsientoDataModel?> ValidarAsientoDisponibleAsync(
        int idVuelo, int idAsiento)
    {
        var asiento = await GetAsientoByIdAsync(idVuelo, idAsiento);

        if (asiento is null)
        {
            _logger.LogWarning(
                "[Bus][VuelosDataService] Asiento no encontrado. " +
                "IdVuelo={IdVuelo} IdAsiento={IdAsiento}",
                idVuelo, idAsiento);
            return null;
        }

        if (!asiento.EsReservable)
        {
            _logger.LogWarning(
                "[Bus][VuelosDataService] Asiento no disponible para reservar. " +
                "IdVuelo={IdVuelo} IdAsiento={IdAsiento} " +
                "Disponible={Disponible} Estado={Estado}",
                idVuelo, idAsiento, asiento.Disponible, asiento.Estado);
            return null;
        }

        return asiento;
    }

    /// <inheritdoc />
    public async Task<bool> BloquearAsientoAsync(
        int idVuelo, int idAsiento, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][VuelosDataService] BloquearAsiento. " +
            "IdVuelo={IdVuelo} IdAsiento={IdAsiento}",
            idVuelo, idAsiento);

        var result = await _vuelosClient.ActualizarDisponibilidadAsientoAsync(
            idVuelo, idAsiento, disponible: false, jwtToken);

        if (result is null)
        {
            _logger.LogError(
                "[Bus][VuelosDataService] BloquearAsiento falló. " +
                "IdVuelo={IdVuelo} IdAsiento={IdAsiento}",
                idVuelo, idAsiento);
            return false;
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> LiberarAsientoAsync(
        int idVuelo, int idAsiento, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][VuelosDataService] LiberarAsiento. " +
            "IdVuelo={IdVuelo} IdAsiento={IdAsiento}",
            idVuelo, idAsiento);

        var result = await _vuelosClient.ActualizarDisponibilidadAsientoAsync(
            idVuelo, idAsiento, disponible: true, jwtToken);

        if (result is null)
        {
            _logger.LogError(
                "[Bus][VuelosDataService] LiberarAsiento falló. " +
                "IdVuelo={IdVuelo} IdAsiento={IdAsiento}",
                idVuelo, idAsiento);
            return false;
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<List<EscalaDataModel>> GetEscalasByVueloAsync(int idVuelo)
    {
        var dtos = await _vuelosClient.GetEscalasByVueloAsync(idVuelo);
        return dtos.Select(MapEscalaToModel).ToList();
    }

    // ── Mappers privados ─────────────────────────────────────────────────────────

    private static VueloDataModel MapVueloToModel(
        DataAccess.Models.VueloDto dto) => new()
        {
            IdVuelo = dto.IdVuelo,
            IdAeropuertoOrigen = dto.IdAeropuertoOrigen,
            IdAeropuertoDestino = dto.IdAeropuertoDestino,
            NumeroVuelo = dto.NumeroVuelo,
            FechaHoraSalida = dto.FechaHoraSalida,
            FechaHoraLlegada = dto.FechaHoraLlegada,
            DuracionMin = dto.DuracionMin,
            PrecioBase = dto.PrecioBase,
            CapacidadTotal = dto.CapacidadTotal,
            EstadoVuelo = dto.EstadoVuelo,
            Estado = dto.Estado,
            Eliminado = dto.Eliminado
        };

    private static AsientoDataModel MapAsientoToModel(
        DataAccess.Models.AsientoDto dto) => new()
        {
            IdAsiento = dto.IdAsiento,
            IdVuelo = dto.IdVuelo,
            NumeroAsiento = dto.NumeroAsiento,
            Clase = dto.Clase,
            Disponible = dto.Disponible,
            PrecioExtra = dto.PrecioExtra,
            Posicion = dto.Posicion,
            Estado = dto.Estado,
            Eliminado = dto.Eliminado
        };

    private static EscalaDataModel MapEscalaToModel(
        DataAccess.Models.EscalaDto dto) => new()
        {
            IdEscala = dto.IdEscala,
            IdVuelo = dto.IdVuelo,
            IdAeropuerto = dto.IdAeropuerto,
            Orden = dto.Orden,
            FechaHoraLlegada = dto.FechaHoraLlegada,
            FechaHoraSalida = dto.FechaHoraSalida,
            DuracionMin = dto.DuracionMin,
            TipoEscala = dto.TipoEscala,
            Terminal = dto.Terminal,
            Puerta = dto.Puerta,
            Observaciones = dto.Observaciones,
            Estado = dto.Estado,
            Eliminado = dto.Eliminado
        };
}