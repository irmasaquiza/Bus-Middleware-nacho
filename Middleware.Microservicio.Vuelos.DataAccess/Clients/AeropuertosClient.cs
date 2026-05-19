using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataAccess.Clients;

/// <summary>
/// Cliente HTTP del Bus hacia MS Aeropuertos.
/// URL dev IIS Express: https://localhost:44363
/// Named HttpClient: "AeropuertosClient"
/// </summary>
public partial class AeropuertosClient : IAeropuertosClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AeropuertosClient> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AeropuertosClient(HttpClient httpClient, ILogger<AeropuertosClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<AeropuertoDto?> GetByIdAsync(int idAeropuerto)
    {
        var endpoint = $"api/v1/aeropuertos/{idAeropuerto}";

        _logger.LogInformation(
            "[Bus->Aeropuertos] GetById. IdAeropuerto={IdAeropuerto}",
            idAeropuerto);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Aeropuertos] Error de conexión en GetById. " +
                "IdAeropuerto={IdAeropuerto}", idAeropuerto);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Aeropuertos.", ex);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "[Bus->Aeropuertos] Aeropuerto no encontrado. IdAeropuerto={IdAeropuerto}",
                idAeropuerto);
            return null;
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<AeropuertosApiResponseDto<AeropuertoDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<AeropuertoDto?> GetByCodigoIataAsync(string codigoIata)
    {
        var endpoint = $"api/v1/aeropuertos?codigoIata={Uri.EscapeDataString(codigoIata)}";

        _logger.LogInformation(
            "[Bus->Aeropuertos] GetByCodigoIata. CodigoIata={CodigoIata}",
            codigoIata);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Aeropuertos] Error de conexión en GetByCodigoIata. " +
                "CodigoIata={CodigoIata}", codigoIata);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Aeropuertos.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<AeropuertosApiResponseDto<AeropuertoDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<AeropuertoDto?> CrearAsync(
        CrearAeropuertoRequestDto request,
        string jwtToken)
    {
        const string endpoint = "api/v1/aeropuertos";

        _logger.LogInformation(
            "[Bus->Aeropuertos] Crear. CodigoIata={CodigoIata} IdPais={IdPais}",
            request.CodigoIata, request.IdPais);

        // Reenviar el JWT del usuario — MS Aeropuertos valida roles ADMINISTRADOR/AEROLINEA
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(requestMessage);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Aeropuertos] Error de conexión en Crear. " +
                "CodigoIata={CodigoIata}", request.CodigoIata);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Aeropuertos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->Aeropuertos] Crear fallido. " +
                "StatusCode={StatusCode} CodigoIata={CodigoIata} Body={Body}",
                (int)response.StatusCode, request.CodigoIata, body);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<AeropuertosApiResponseDto<AeropuertoDto>>(body, _jsonOptions);

        if (apiResponse?.Success == true)
        {
            _logger.LogInformation(
                "[Bus->Aeropuertos] Crear exitoso. " +
                "IdAeropuerto={IdAeropuerto} CodigoIata={CodigoIata}",
                apiResponse.Data?.IdAeropuerto, request.CodigoIata);
        }

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<bool> EliminarAsync(int idAeropuerto, string jwtToken)
    {
        var endpoint = $"api/v1/aeropuertos/{idAeropuerto}";

        _logger.LogInformation(
            "[Bus->Aeropuertos] Eliminar. IdAeropuerto={IdAeropuerto}",
            idAeropuerto);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(requestMessage);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Aeropuertos] Error de conexión en Eliminar. " +
                "IdAeropuerto={IdAeropuerto}", idAeropuerto);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Aeropuertos.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "[Bus->Aeropuertos] Eliminar fallido. " +
                "StatusCode={StatusCode} IdAeropuerto={IdAeropuerto}",
                (int)response.StatusCode, idAeropuerto);
            return false;
        }

        return true;
    }
}