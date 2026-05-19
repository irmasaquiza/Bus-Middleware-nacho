using Middleware.Vuelos.DataAccess.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Clients;

public partial class VuelosClient
{
    // ── Vuelos Admin ──────────────────────────────────────────────────────────

    public async Task<VuelosPagedResultDto<VueloDto>?> GetVuelosPagedAsync(
        int? idAeropuertoOrigen, int? idAeropuertoDestino,
        string? numeroVuelo, string? estadoVuelo,
        int page, int pageSize)
    {
        var query = $"api/v1/vuelos?page={page}&page_size={pageSize}";
        if (idAeropuertoOrigen.HasValue) query += $"&id_aeropuerto_origen={idAeropuertoOrigen}";
        if (idAeropuertoDestino.HasValue) query += $"&id_aeropuerto_destino={idAeropuertoDestino}";
        if (!string.IsNullOrWhiteSpace(numeroVuelo)) query += $"&numero_vuelo={numeroVuelo}";
        if (!string.IsNullOrWhiteSpace(estadoVuelo)) query += $"&estado_vuelo={estadoVuelo}";

        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(query); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();


        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VuelosPagedResultDto<VueloDto>>>(body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<VueloDto?> CrearVueloAsync(
        CrearVueloRequestDto request, string jwtToken)
    {
        const string endpoint = "api/v1/vuelos";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VueloDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<VueloDto?> ActualizarVueloAsync(
        int idVuelo, ActualizarVueloRequestDto request, string jwtToken)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VueloDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<VueloDto?> CambiarEstadoVueloAsync(
        int idVuelo, string estadoVuelo, string jwtToken)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/estado";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(new { estadoVuelo });

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VueloDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> EliminarVueloAsync(int idVuelo, string jwtToken)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}";
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }
        return response.IsSuccessStatusCode;
    }

    // ── Escalas Admin ─────────────────────────────────────────────────────────

    public async Task<EscalaDto?> GetEscalaByIdAsync(int idVuelo, int idEscala)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/escalas/{idEscala}";
        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(endpoint); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<EscalaDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<EscalaDto?> CrearEscalaAsync(
        int idVuelo, CrearEscalaRequestDto request, string jwtToken)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/escalas";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<EscalaDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> EliminarEscalaAsync(
        int idVuelo, int idEscala, string jwtToken)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/escalas/{idEscala}";
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }
        return response.IsSuccessStatusCode;
    }

    // ── Asientos Admin ────────────────────────────────────────────────────────

    public async Task<AsientoDto?> GetAsientoByIdAdminAsync(int idVuelo, int idAsiento)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/asientos/{idAsiento}";
        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(endpoint); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<AsientoDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<AsientoDto?> CrearAsientoAsync(
        int idVuelo, CrearAsientoRequestDto request, string jwtToken)
    {
        var endpoint = $"api/v1/vuelos/{idVuelo}/asientos";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<AsientoDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }
}

// ── DTOs nuevos de Vuelos Admin ───────────────────────────────────────────────
