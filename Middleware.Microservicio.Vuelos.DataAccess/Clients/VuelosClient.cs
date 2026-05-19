using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataAccess.Clients;

/// <summary>
/// Cliente HTTP del Bus hacia MS Vuelos.
/// URL dev IIS Express: https://localhost:44385
/// Named HttpClient: "VuelosClient"
/// </summary>
public partial class VuelosClient : IVuelosClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VuelosClient> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public VuelosClient(HttpClient httpClient, ILogger<VuelosClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<VueloDto?> GetVueloByIdAsync(int idVuelo)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}";

        _logger.LogInformation(
            "[Bus->Vuelos] GetVueloById. IdVuelo={IdVuelo}", idVuelo);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Vuelos] Error de conexión en GetVueloById. IdVuelo={IdVuelo}",
                idVuelo);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "[Bus->Vuelos] Vuelo no encontrado. IdVuelo={IdVuelo}", idVuelo);
            return null;
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VueloDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<List<AsientoDto>> GetAsientosByVueloAsync(int idVuelo)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/asientos";

        _logger.LogInformation(
            "[Bus->Vuelos] GetAsientosByVuelo. IdVuelo={IdVuelo}", idVuelo);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Vuelos] Error de conexión en GetAsientosByVuelo. " +
                "IdVuelo={IdVuelo}", idVuelo);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return [];

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VuelosPagedResultDto<AsientoDto>>>(body, _jsonOptions);

        return apiResponse?.Success == true
            ? apiResponse.Data?.Items ?? []
            : [];
    }

    /// <inheritdoc />
    public async Task<AsientoDto?> GetAsientoByIdAsync(int idVuelo, int idAsiento)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/asientos/{idAsiento}";

        _logger.LogInformation(
            "[Bus->Vuelos] GetAsientoById. IdVuelo={IdVuelo} IdAsiento={IdAsiento}",
            idVuelo, idAsiento);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Vuelos] Error de conexión en GetAsientoById. " +
                "IdVuelo={IdVuelo} IdAsiento={IdAsiento}", idVuelo, idAsiento);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<AsientoDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<List<EscalaDto>> GetEscalasByVueloAsync(int idVuelo)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/escalas";

        _logger.LogInformation(
            "[Bus->Vuelos] GetEscalasByVuelo. IdVuelo={IdVuelo}", idVuelo);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Vuelos] Error de conexión en GetEscalasByVuelo. " +
                "IdVuelo={IdVuelo}", idVuelo);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return [];

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VuelosPagedResultDto<EscalaDto>>>(body, _jsonOptions);

        return apiResponse?.Success == true
            ? apiResponse.Data?.Items ?? []
            : [];
    }

    /// <inheritdoc />
    public async Task<AsientoDto?> ActualizarDisponibilidadAsientoAsync(
        int idVuelo,
        int idAsiento,
        bool disponible,
        string jwtToken)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/asientos/{idAsiento}";

        _logger.LogInformation(
            "[Bus->Vuelos] ActualizarDisponibilidadAsiento. " +
            "IdVuelo={IdVuelo} IdAsiento={IdAsiento} Disponible={Disponible}",
            idVuelo, idAsiento, disponible);

        using var requestMessage = new HttpRequestMessage(
            HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(
            new ActualizarAsientoRequestDto { Disponible = disponible });

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(requestMessage);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Vuelos] Error de conexión en ActualizarDisponibilidadAsiento. " +
                "IdVuelo={IdVuelo} IdAsiento={IdAsiento}", idVuelo, idAsiento);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->Vuelos] ActualizarDisponibilidadAsiento fallido. " +
                "StatusCode={StatusCode} IdVuelo={IdVuelo} IdAsiento={IdAsiento}",
                (int)response.StatusCode, idVuelo, idAsiento);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<AsientoDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }
}