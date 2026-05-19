using Microsoft.Extensions.Logging;
using Middleware.Vuelos.DataAccess.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Middleware.Vuelos.DataAccess.Clients;

/// <summary>
/// Extensión del cliente HTTP de Seguridad con operaciones admin.
/// Agrega los métodos faltantes: logout, register, usuarios CRUD, roles.
/// </summary>
public partial class SeguridadClient
{
    // ── Auth ──────────────────────────────────────────────────────────────────

    public async Task<bool> LogoutAsync(string jwtToken)
    {
        const string endpoint = "api/v1/auth/logout";
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }
        return response.IsSuccessStatusCode;
    }

    public async Task<RegisterClienteResponseDto?> RegisterClienteAsync(
        RegisterClienteRequestDto request)
    {

        // ← Agrega este log temporal
        _logger.LogInformation(
            "[Bus->Seguridad] RegisterCliente. Username={Username} IdCliente={IdCliente}",
            request.Username, request.IdCliente);

        const string endpoint = "api/v1/auth/register-cliente";
        HttpResponseMessage response;
        try { response = await _httpClient.PostAsJsonAsync(endpoint, request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;

        var apiResponse = JsonSerializer
            .Deserialize<SeguridadApiResponseDto<RegisterClienteResponseDto>>(
                body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    // ── Usuarios Admin ────────────────────────────────────────────────────────

    public async Task<SeguridadPagedResponseDto<UsuarioResponseDto>?> GetUsuariosAsync(
        string? username, string? correo, bool? activo,
        int page, int pageSize, string jwtToken)
    {
        var query = $"api/v1/usuarios?page={page}&page_size={pageSize}";
        if (!string.IsNullOrWhiteSpace(username)) query += $"&username={username}";
        if (!string.IsNullOrWhiteSpace(correo)) query += $"&correo={correo}";
        if (activo.HasValue) query += $"&activo={activo.Value}";

        using var request = new HttpRequestMessage(HttpMethod.Get, query);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<SeguridadApiResponseDto<SeguridadPagedResponseDto<UsuarioResponseDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<UsuarioResponseDto?> GetUsuarioByIdAsync(
        int idUsuario, string jwtToken)
    {
        var endpoint = $"api/v1/usuarios/{idUsuario}";
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<SeguridadApiResponseDto<UsuarioResponseDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<UsuarioResponseDto?> CrearUsuarioAsync(
        CrearUsuarioRequestDto request, string jwtToken)
    {
        const string endpoint = "api/v1/usuarios";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<SeguridadApiResponseDto<UsuarioResponseDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<UsuarioResponseDto?> ActualizarUsuarioAsync(
        int idUsuario, ActualizarUsuarioRequestDto request, string jwtToken)
    {
        var endpoint = $"api/v1/usuarios/{idUsuario}";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<SeguridadApiResponseDto<UsuarioResponseDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> EliminarUsuarioAsync(int idUsuario, string jwtToken)
    {
        var endpoint = $"api/v1/usuarios/{idUsuario}";
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }
        return response.IsSuccessStatusCode;
    }

    // ── Roles Admin ───────────────────────────────────────────────────────────

    public async Task<UsuarioRolResponseDto?> AsignarRolAsync(
        int idUsuario, AsignarRolRequestDto request, string jwtToken)
    {
        var endpoint = $"api/v1/usuarios/{idUsuario}/roles";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<SeguridadApiResponseDto<UsuarioRolResponseDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> RemoverRolAsync(
        int idUsuario, int idRol, string jwtToken)
    {
        var endpoint = $"api/v1/usuarios/{idUsuario}/roles/{idRol}";
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad.", ex);
        }
        return response.IsSuccessStatusCode;
    }
}
