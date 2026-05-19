using System.Net.Http.Json;
using System.Text.Json;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataAccess.Clients;

/// <summary>
/// Cliente HTTP del Bus hacia MS Seguridad.
/// Llama a los endpoints REST reales del microservicio.
///
/// URLs de desarrollo:
///   HTTP:  http://localhost:5062
///   HTTPS: https://localhost:7195
///
/// Named HttpClient esperado: "SeguridadClient"
/// Configurado en HttpClientExtensions.cs del Bus.
/// </summary>
public partial class SeguridadClient : ISeguridadClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SeguridadClient> _logger;

    // Opciones de serialización consistentes con lo que MS Seguridad devuelve.
    // ASP.NET Core por defecto serializa en camelCase.
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public SeguridadClient(HttpClient httpClient, ILogger<SeguridadClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        const string endpoint = "api/v1/auth/login";

        _logger.LogInformation(
            "[Bus->Seguridad] Login. Username={Username} Endpoint={Endpoint}",
            request.Username, endpoint);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(endpoint, request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Seguridad] Error de conexión en Login. Endpoint={Endpoint}",
                endpoint);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad para login.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "[Bus->Seguridad] Login fallido. StatusCode={StatusCode} Body={Body}",
                (int)response.StatusCode, body);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<SeguridadApiResponseDto<LoginResponseDto>>(body, _jsonOptions);

        if (apiResponse?.Success != true || apiResponse.Data is null)
        {
            _logger.LogWarning(
                "[Bus->Seguridad] Login: respuesta sin datos. Message={Message}",
                apiResponse?.Message);
            return null;
        }

        _logger.LogInformation(
            "[Bus->Seguridad] Login exitoso. Usuario={Usuario} Roles={Roles}",
            apiResponse.Data.Usuario,
            string.Join(",", apiResponse.Data.Roles));

        return apiResponse.Data;
    }

    /// <inheritdoc />
    public async Task<CreateUserForClientResponseDto?> CreateUserForClientAsync(
        CreateUserForClientRequestDto request)
    {
        const string endpoint = "api/v1/internal/seguridad/users/create-for-client";

        _logger.LogInformation(
            "[Bus->Seguridad] CreateUserForClient. " +
            "IdCliente={IdCliente} Username={Username} CorrelationId={CorrelationId}",
            request.IdCliente, request.Username, request.CorrelationId);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(endpoint, request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Seguridad] Error de conexión en CreateUserForClient. " +
                "IdCliente={IdCliente} CorrelationId={CorrelationId}",
                request.IdCliente, request.CorrelationId);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Seguridad para crear usuario.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();

        // 409 Conflict = username o correo ya en uso por otro IdCliente. Error de negocio.
        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            _logger.LogWarning(
                "[Bus->Seguridad] CreateUserForClient: conflicto de username/correo. " +
                "IdCliente={IdCliente} Body={Body}",
                request.IdCliente, body);
            throw new InvalidOperationException(
                $"Conflicto al crear usuario en Seguridad: {body}");
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->Seguridad] CreateUserForClient fallido. " +
                "StatusCode={StatusCode} IdCliente={IdCliente} Body={Body}",
                (int)response.StatusCode, request.IdCliente, body);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<SeguridadApiResponseDto<CreateUserForClientResponseDto>>(
                body, _jsonOptions);

        if (apiResponse?.Success != true || apiResponse.Data is null)
        {
            _logger.LogWarning(
                "[Bus->Seguridad] CreateUserForClient: respuesta sin datos. " +
                "Message={Message}",
                apiResponse?.Message);
            return null;
        }

        _logger.LogInformation(
            "[Bus->Seguridad] CreateUserForClient exitoso. " +
            "IdUsuario={IdUsuario} IdCliente={IdCliente} RolAsignado={RolAsignado}",
            apiResponse.Data.IdUsuario,
            apiResponse.Data.IdCliente,
            apiResponse.Data.RolAsignado);

        return apiResponse.Data;
    }
}