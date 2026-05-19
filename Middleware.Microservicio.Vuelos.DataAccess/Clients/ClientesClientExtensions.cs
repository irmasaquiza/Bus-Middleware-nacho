using Middleware.Vuelos.DataAccess.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Clients;

public partial class ClientesClient
{
    // ── Clientes Admin ────────────────────────────────────────────────────────

    public async Task<ClientesPagedResponseDto<ClienteDto>?> GetClientesPagedAsync(
        string? tipoIdentificacion, string? numeroIdentificacion,
        string? nombres, string? correo, string? estado,
        int page, int pageSize, string jwtToken)
    {
        var query = $"api/v1/clientes?page={page}&page_size={pageSize}";
        if (!string.IsNullOrWhiteSpace(tipoIdentificacion)) query += $"&tipo_identificacion={tipoIdentificacion}";
        if (!string.IsNullOrWhiteSpace(numeroIdentificacion)) query += $"&numero_identificacion={numeroIdentificacion}";
        if (!string.IsNullOrWhiteSpace(nombres)) query += $"&nombres={nombres}";
        if (!string.IsNullOrWhiteSpace(correo)) query += $"&correo={correo}";
        if (!string.IsNullOrWhiteSpace(estado)) query += $"&estado={estado}";

        using var request = new HttpRequestMessage(HttpMethod.Get, query);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<ClientesPagedResponseDto<ClienteDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<ClienteDto?> ActualizarClienteAsync(
        int idCliente, ActualizarClienteRequestDto request, string jwtToken)
    {
        var endpoint = $"api/v1/clientes/{idCliente}";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<ClienteDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> EliminarClienteAsync(int idCliente, string jwtToken)
    {
        var endpoint = $"api/v1/clientes/{idCliente}";
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }
        return response.IsSuccessStatusCode;
    }

    // ── Portal Cliente ────────────────────────────────────────────────────────

    public async Task<ClienteDto?> GetMiPerfilAsync(string jwtToken)
    {
        const string endpoint = "api/v1/clientes/portal/mi-perfil";
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<ClienteDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<ClienteDto?> ActualizarMiPerfilAsync(
        ActualizarClienteRequestDto request, string jwtToken)
    {
        const string endpoint = "api/v1/clientes/portal/mi-perfil";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<ClienteDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    // ── Pasajeros Admin ───────────────────────────────────────────────────────

    public async Task<ClientesPagedResponseDto<PasajeroDto>?> GetPasajerosPagedAsync(
        int? idCliente, string? nombrePasajero, string? numeroDocumento,
        string? estado, int page, int pageSize, string jwtToken)
    {
        var query = $"api/v1/pasajeros?page={page}&page_size={pageSize}";
        if (idCliente.HasValue) query += $"&id_cliente={idCliente}";
        if (!string.IsNullOrWhiteSpace(nombrePasajero)) query += $"&nombre_pasajero={nombrePasajero}";
        if (!string.IsNullOrWhiteSpace(numeroDocumento)) query += $"&numero_documento={numeroDocumento}";
        if (!string.IsNullOrWhiteSpace(estado)) query += $"&estado={estado}";

        using var request = new HttpRequestMessage(HttpMethod.Get, query);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<ClientesPagedResponseDto<PasajeroDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<PasajeroDto?> ActualizarPasajeroAsync(
        int idPasajero, ActualizarPasajeroRequestDto request, string jwtToken)
    {
        var endpoint = $"api/v1/pasajeros/{idPasajero}";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<PasajeroDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }
    /// <summary>
    /// Crea un cliente sin autenticación usando el endpoint público del portal.
    /// POST /api/v1/clientes/portal/registro
    /// </summary>
    public async Task<ClienteDto?> RegistrarClientePublicoAsync(
        CrearClienteRequestDto request)
    {
        const string endpoint = "api/v1/clientes/portal/registro";

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;

        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<ClienteDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

}

// ── DTOs nuevos de Clientes ───────────────────────────────────────────────────
