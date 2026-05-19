using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataAccess.Clients;

/// <summary>
/// Cliente HTTP del Bus hacia MS Clientes.
/// URL dev IIS Express: https://localhost:44391
/// Named HttpClient: "ClientesClient"
/// </summary>
public partial class ClientesClient : IClientesClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ClientesClient> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ClientesClient(HttpClient httpClient, ILogger<ClientesClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ClienteDto?> GetClienteByIdAsync(int idCliente, string jwtToken)
    {
        var endpoint = $"api/v1/clientes/{idCliente}";

        _logger.LogInformation(
            "[Bus->Clientes] GetClienteById. IdCliente={IdCliente}",
            idCliente);

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Clientes] Error de conexión en GetClienteById. " +
                "IdCliente={IdCliente}", idCliente);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "[Bus->Clientes] Cliente no encontrado. IdCliente={IdCliente}",
                idCliente);
            return null;
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<ClienteDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<ClienteDto?> CrearClienteAsync(
        CrearClienteRequestDto request,
        string jwtToken)
    {
        const string endpoint = "api/v1/clientes";

        _logger.LogInformation(
            "[Bus->Clientes] CrearCliente. " +
            "NumeroIdentificacion={NumeroIdentificacion} Correo={Correo}",
            request.NumeroIdentificacion, request.Correo);

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
                "[Bus->Clientes] Error de conexión en CrearCliente. " +
                "Correo={Correo}", request.Correo);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->Clientes] CrearCliente fallido. " +
                "StatusCode={StatusCode} Body={Body}",
                (int)response.StatusCode, body);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<ClienteDto>>(body, _jsonOptions);

        if (apiResponse?.Success == true)
        {
            _logger.LogInformation(
                "[Bus->Clientes] CrearCliente exitoso. IdCliente={IdCliente}",
                apiResponse.Data?.IdCliente);
        }

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<PasajeroDto?> GetPasajeroByIdAsync(int idPasajero, string jwtToken)
    {
        var endpoint = $"api/v1/pasajeros/{idPasajero}";

        _logger.LogInformation(
            "[Bus->Clientes] GetPasajeroById. IdPasajero={IdPasajero}",
            idPasajero);

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->Clientes] Error de conexión en GetPasajeroById. " +
                "IdPasajero={IdPasajero}", idPasajero);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "[Bus->Clientes] Pasajero no encontrado. IdPasajero={IdPasajero}",
                idPasajero);
            return null;
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<PasajeroDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<PasajeroDto?> CrearPasajeroAsync(
        CrearPasajeroRequestDto request,
        string jwtToken)
    {
        const string endpoint = "api/v1/pasajeros";

        _logger.LogInformation(
            "[Bus->Clientes] CrearPasajero. " +
            "IdCliente={IdCliente} NumeroDocumento={NumeroDocumento}",
            request.IdCliente, request.NumeroDocumentoPasajero);

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
                "[Bus->Clientes] Error de conexión en CrearPasajero. " +
                "IdCliente={IdCliente}", request.IdCliente);
            throw new InvalidOperationException(
                "No se pudo conectar con MS Clientes.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->Clientes] CrearPasajero fallido. " +
                "StatusCode={StatusCode} Body={Body}",
                (int)response.StatusCode, body);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<ClientesApiResponseDto<PasajeroDto>>(body, _jsonOptions);

        if (apiResponse?.Success == true)
        {
            _logger.LogInformation(
                "[Bus->Clientes] CrearPasajero exitoso. IdPasajero={IdPasajero}",
                apiResponse.Data?.IdPasajero);
        }

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }
}