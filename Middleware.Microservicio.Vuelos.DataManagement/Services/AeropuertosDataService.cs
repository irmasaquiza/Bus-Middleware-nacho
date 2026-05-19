using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Models.Aeropuertos;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataManagement.Services;

/// <summary>
/// Implementación del servicio de datos de Aeropuertos en el Bus.
/// Llama al AeropuertosClient (HTTP) y transforma DTOs en modelos internos.
/// </summary>
public class AeropuertosDataService : IAeropuertosDataService
{
    private readonly IAeropuertosClient _aeropuertosClient;
    private readonly ILogger<AeropuertosDataService> _logger;

    public AeropuertosDataService(
        IAeropuertosClient aeropuertosClient,
        ILogger<AeropuertosDataService> logger)
    {
        _aeropuertosClient = aeropuertosClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<AeropuertoDataModel?> GetByIdAsync(int idAeropuerto)
    {
        var dto = await _aeropuertosClient.GetByIdAsync(idAeropuerto);
        return dto is null ? null : MapToModel(dto);
    }

    /// <inheritdoc />
    public async Task<AeropuertoDataModel?> GetByCodigoIataAsync(string codigoIata)
    {
        var dto = await _aeropuertosClient.GetByCodigoIataAsync(codigoIata);
        return dto is null ? null : MapToModel(dto);
    }

    /// <inheritdoc />
    public async Task<AeropuertoDataModel?> CrearAsync(
        string codigoIata,
        string? codigoIcao,
        string nombre,
        int idPais,
        int? idCiudad,
        string? zonaHoraria,
        decimal? latitud,
        decimal? longitud,
        string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][AeropuertosDataService] Crear aeropuerto. " +
            "CodigoIata={CodigoIata} IdPais={IdPais} IdCiudad={IdCiudad}",
            codigoIata, idPais, idCiudad);

        var dto = await _aeropuertosClient.CrearAsync(
            new CrearAeropuertoRequestDto
            {
                CodigoIata = codigoIata,
                CodigoIcao = codigoIcao,
                Nombre = nombre,
                IdPais = idPais,
                IdCiudad = idCiudad,
                ZonaHoraria = zonaHoraria,
                Latitud = latitud,
                Longitud = longitud
            },
            jwtToken);

        if (dto is null)
        {
            _logger.LogWarning(
                "[Bus][AeropuertosDataService] Crear devolvió null. " +
                "CodigoIata={CodigoIata}", codigoIata);
            return null;
        }

        return MapToModel(dto);
    }

    /// <inheritdoc />
    public async Task<bool> EliminarAsync(int idAeropuerto, string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][AeropuertosDataService] Eliminar aeropuerto. " +
            "IdAeropuerto={IdAeropuerto}", idAeropuerto);

        return await _aeropuertosClient.EliminarAsync(idAeropuerto, jwtToken);
    }

    // ── Mapper privado ───────────────────────────────────────────────────────────

    private static AeropuertoDataModel MapToModel(AeropuertoDto dto) =>
        new()
        {
            IdAeropuerto = dto.IdAeropuerto,
            CodigoIata = dto.CodigoIata,
            CodigoIcao = dto.CodigoIcao,
            Nombre = dto.Nombre,
            IdCiudad = dto.IdCiudad,
            IdPais = dto.IdPais,
            ZonaHoraria = dto.ZonaHoraria,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            Estado = dto.Estado,
            Eliminado = dto.Eliminado
        };
}