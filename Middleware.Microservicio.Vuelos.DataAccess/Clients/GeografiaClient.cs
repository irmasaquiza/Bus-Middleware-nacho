using System.Net.Http.Json;
using System.Text.Json;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataAccess.Clients;

/// <summary>
/// Cliente HTTP del Bus hacia MS Geografía.
/// Llama a los endpoints REST reales del microservicio.
///
/// URL dev IIS Express: https://localhost:44395
/// Named HttpClient: "GeografiaClient"
/// Configurado en HttpClientGeografiaExtensions.cs
/// </summary>
public partial class GeografiaClient : IGeografiaClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeografiaClient> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public GeografiaClient(HttpClient httpClient, ILogger<GeografiaClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<PaisDto?> GetPaisByIdAsync(int idPais)
    {
        var endpoint = $"api/v1/paises/{idPais}";

        _logger.LogInformation(
            "[Bus->Geografia] GetPaisById. IdPais={IdPais}",
            idPais);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Geografia] Error de conexión en GetPaisById. IdPais={IdPais}",
                idPais);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "[Bus->Geografia] País no encontrado. IdPais={IdPais}", idPais);
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->Geografia] GetPaisById fallido. StatusCode={StatusCode} IdPais={IdPais}",
                (int)response.StatusCode, idPais);
            return null;
        }

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<PaisDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<PaisDto?> GetPaisByCodigoIso2Async(string codigoIso2)
    {
        var endpoint = $"api/v1/paises?codigoIso2={Uri.EscapeDataString(codigoIso2)}";

        _logger.LogInformation(
            "[Bus->Geografia] GetPaisByCodigoIso2. CodigoIso2={CodigoIso2}",
            codigoIso2);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Geografia] Error de conexión en GetPaisByCodigoIso2. " +
                "CodigoIso2={CodigoIso2}", codigoIso2);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "[Bus->Geografia] GetPaisByCodigoIso2 fallido. " +
                "StatusCode={StatusCode} CodigoIso2={CodigoIso2}",
                (int)response.StatusCode, codigoIso2);
            return null;
        }

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<PaisDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<CiudadDto?> GetCiudadByIdAsync(int idCiudad)
    {
        var endpoint = $"api/v1/ciudades/{idCiudad}";

        _logger.LogInformation(
            "[Bus->Geografia] GetCiudadById. IdCiudad={IdCiudad}",
            idCiudad);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Geografia] Error de conexión en GetCiudadById. IdCiudad={IdCiudad}",
                idCiudad);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "[Bus->Geografia] Ciudad no encontrada. IdCiudad={IdCiudad}", idCiudad);
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->Geografia] GetCiudadById fallido. " +
                "StatusCode={StatusCode} IdCiudad={IdCiudad}",
                (int)response.StatusCode, idCiudad);
            return null;
        }

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<CiudadDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<bool> ValidarCiudadPerteneceAPaisAsync(int idCiudad, int idPais)
    {
        _logger.LogInformation(
            "[Bus->Geografia] ValidarCiudadPerteneceAPais. " +
            "IdCiudad={IdCiudad} IdPais={IdPais}",
            idCiudad, idPais);

        var ciudad = await GetCiudadByIdAsync(idCiudad);

        if (ciudad is null)
        {
            _logger.LogWarning(
                "[Bus->Geografia] Validación fallida: ciudad no existe. " +
                "IdCiudad={IdCiudad}", idCiudad);
            return false;
        }

        if (ciudad.Eliminado || ciudad.Estado != "ACT")
        {
            _logger.LogWarning(
                "[Bus->Geografia] Validación fallida: ciudad inactiva o eliminada. " +
                "IdCiudad={IdCiudad} Estado={Estado}",
                idCiudad, ciudad.Estado);
            return false;
        }

        if (ciudad.IdPais != idPais)
        {
            _logger.LogWarning(
                "[Bus->Geografia] Validación fallida: ciudad no pertenece al país. " +
                "IdCiudad={IdCiudad} IdPaisEsperado={IdPaisEsperado} IdPaisReal={IdPaisReal}",
                idCiudad, idPais, ciudad.IdPais);
            return false;
        }

        return true;
    }
}