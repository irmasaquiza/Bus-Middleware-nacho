using Microsoft.Extensions.Logging;
using Middleware.Vuelos.DataAccess.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Middleware.Vuelos.DataAccess.Clients;

/// <summary>
/// Extensión del cliente HTTP de Geografía con CRUD completo.
/// Agrega los métodos faltantes: listar, crear, actualizar y eliminar
/// países y ciudades.
/// </summary>
public partial class GeografiaClient
{
    // ── Países ────────────────────────────────────────────────────────────────

    public async Task<GeografiaPagedResponseDto<PaisDto>?> GetPaisesAsync(
        string? nombre, string? codigoIso2, string? continente,
        string? estado, int paginaActual, int tamanoPagina)
    {
        var query = $"api/v1/paises?PaginaActual={paginaActual}&TamanoPagina={tamanoPagina}";
        if (!string.IsNullOrWhiteSpace(nombre)) query += $"&Nombre={nombre}";
        if (!string.IsNullOrWhiteSpace(codigoIso2)) query += $"&CodigoIso2={codigoIso2}";
        if (!string.IsNullOrWhiteSpace(continente)) query += $"&Continente={continente}";
        if (!string.IsNullOrWhiteSpace(estado)) query += $"&Estado={estado}";

        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(query); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<GeografiaPagedResponseDto<PaisDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<PaisDto?> CrearPaisAsync(CrearPaisRequestDto request)
    {
        const string endpoint = "api/v1/paises";
        HttpResponseMessage response;
        try { response = await _httpClient.PostAsJsonAsync(endpoint, request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<PaisDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<PaisDto?> ActualizarPaisAsync(
        int idPais, ActualizarPaisRequestDto request)
    {
        var endpoint = $"api/v1/paises/{idPais}";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<PaisDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> EliminarPaisAsync(int idPais)
    {
        var endpoint = $"api/v1/paises/{idPais}";
        HttpResponseMessage response;
        try { response = await _httpClient.DeleteAsync(endpoint); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }
        return response.IsSuccessStatusCode;
    }

    // ── Ciudades ──────────────────────────────────────────────────────────────

    public async Task<GeografiaPagedResponseDto<CiudadDto>?> GetCiudadesAsync(
        int? idPais, string? nombre, string? estado,
        int page, int pageSize)
    {
        var query = $"api/v1/ciudades?page={page}&page_size={pageSize}";
        if (idPais.HasValue) query += $"&id_pais={idPais}";
        if (!string.IsNullOrWhiteSpace(nombre)) query += $"&nombre={nombre}";
        if (!string.IsNullOrWhiteSpace(estado)) query += $"&estado={estado}";

        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(query); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<GeografiaPagedResponseDto<CiudadDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<CiudadDto?> CrearCiudadAsync(CrearCiudadRequestDto request)
    {
        const string endpoint = "api/v1/ciudades";
        HttpResponseMessage response;
        try { response = await _httpClient.PostAsJsonAsync(endpoint, request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<CiudadDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<CiudadDto?> ActualizarCiudadAsync(
        int idCiudad, ActualizarCiudadRequestDto request)
    {
        var endpoint = $"api/v1/ciudades/{idCiudad}";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<GeografiaApiResponseDto<CiudadDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> EliminarCiudadAsync(int idCiudad)
    {
        var endpoint = $"api/v1/ciudades/{idCiudad}";
        HttpResponseMessage response;
        try { response = await _httpClient.DeleteAsync(endpoint); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Geografía.", ex);
        }
        return response.IsSuccessStatusCode;
    }
}

// ── DTOs nuevos de Geografía ──────────────────────────────────────────────────
