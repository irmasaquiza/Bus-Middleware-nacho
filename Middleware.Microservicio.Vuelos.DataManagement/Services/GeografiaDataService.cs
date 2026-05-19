using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Models.Geografia;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataManagement.Services;

/// <summary>
/// Implementación del servicio de datos de Geografía en el Bus.
/// Llama al GeografiaClient (HTTP) y transforma los DTOs de DataAccess
/// en modelos internos del Bus (DataManagement).
/// </summary>
public class GeografiaDataService : IGeografiaDataService
{
    private readonly IGeografiaClient _geografiaClient;
    private readonly ILogger<GeografiaDataService> _logger;

    public GeografiaDataService(
        IGeografiaClient geografiaClient,
        ILogger<GeografiaDataService> logger)
    {
        _geografiaClient = geografiaClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<PaisDataModel?> GetPaisByIdAsync(int idPais)
    {
        var dto = await _geografiaClient.GetPaisByIdAsync(idPais);
        if (dto is null) return null;

        return new PaisDataModel
        {
            IdPais = dto.IdPais,
            CodigoIso2 = dto.CodigoIso2,
            CodigoIso3 = dto.CodigoIso3,
            Nombre = dto.Nombre,
            Continente = dto.Continente,
            Estado = dto.Estado,
            Eliminado = dto.Eliminado
        };
    }

    /// <inheritdoc />
    public async Task<PaisDataModel?> GetPaisByCodigoIso2Async(string codigoIso2)
    {
        var dto = await _geografiaClient.GetPaisByCodigoIso2Async(codigoIso2);
        if (dto is null) return null;

        return new PaisDataModel
        {
            IdPais = dto.IdPais,
            CodigoIso2 = dto.CodigoIso2,
            CodigoIso3 = dto.CodigoIso3,
            Nombre = dto.Nombre,
            Continente = dto.Continente,
            Estado = dto.Estado,
            Eliminado = dto.Eliminado
        };
    }

    /// <inheritdoc />
    public async Task<CiudadDataModel?> GetCiudadByIdAsync(int idCiudad)
    {
        var dto = await _geografiaClient.GetCiudadByIdAsync(idCiudad);
        if (dto is null) return null;

        return new CiudadDataModel
        {
            IdCiudad = dto.IdCiudad,
            IdPais = dto.IdPais,
            Nombre = dto.Nombre,
            CodigoPostal = dto.CodigoPostal,
            ZonaHoraria = dto.ZonaHoraria,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            Estado = dto.Estado,
            Eliminado = dto.Eliminado
        };
    }

    /// <inheritdoc />
    public async Task<PaisDataModel?> ValidarPaisActivoAsync(int idPais)
    {
        var pais = await GetPaisByIdAsync(idPais);

        if (pais is null)
        {
            _logger.LogWarning(
                "[Bus][GeografiaDataService] País no encontrado. IdPais={IdPais}",
                idPais);
            return null;
        }

        if (!pais.EsActivo)
        {
            _logger.LogWarning(
                "[Bus][GeografiaDataService] País inactivo o eliminado. " +
                "IdPais={IdPais} Estado={Estado} Eliminado={Eliminado}",
                idPais, pais.Estado, pais.Eliminado);
            return null;
        }

        return pais;
    }

    /// <inheritdoc />
    public async Task<CiudadDataModel?> ValidarCiudadActivaEnPaisAsync(
        int idCiudad, int idPais)
    {
        var ciudad = await GetCiudadByIdAsync(idCiudad);

        if (ciudad is null)
        {
            _logger.LogWarning(
                "[Bus][GeografiaDataService] Ciudad no encontrada. IdCiudad={IdCiudad}",
                idCiudad);
            return null;
        }

        if (!ciudad.EsActiva)
        {
            _logger.LogWarning(
                "[Bus][GeografiaDataService] Ciudad inactiva o eliminada. " +
                "IdCiudad={IdCiudad} Estado={Estado} Eliminado={Eliminado}",
                idCiudad, ciudad.Estado, ciudad.Eliminado);
            return null;
        }

        if (ciudad.IdPais != idPais)
        {
            _logger.LogWarning(
                "[Bus][GeografiaDataService] Ciudad no pertenece al país. " +
                "IdCiudad={IdCiudad} IdPaisEsperado={IdPaisEsperado} IdPaisReal={IdPaisReal}",
                idCiudad, idPais, ciudad.IdPais);
            return null;
        }

        return ciudad;
    }
}