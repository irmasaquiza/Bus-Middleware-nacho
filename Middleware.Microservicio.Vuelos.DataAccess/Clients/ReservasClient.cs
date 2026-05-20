using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataAccess.Clients;

/// <summary>
/// Cliente HTTP del Bus hacia MS ReservasF.
/// URL dev IIS Express: https://localhost:44370
/// Named HttpClient: "ReservasFClient"
/// </summary>
public partial class ReservasFClient : IReservasFClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReservasFClient> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ReservasFClient(HttpClient httpClient, ILogger<ReservasFClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    // ── RESERVAS ─────────────────────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<ReservaDto?> GetReservaByIdAsync(int idReserva, string jwtToken)
    {
        var endpoint = $"api/v1/reservas/{idReserva}";

        _logger.LogInformation(
            "[Bus->ReservasF] GetReservaById. IdReserva={IdReserva}", idReserva);

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
                "[Bus->ReservasF] Error de conexión en GetReservaById. " +
                "IdReserva={IdReserva}", idReserva);
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning(
                "[Bus->ReservasF] Reserva no encontrada. IdReserva={IdReserva}",
                idReserva);
            return null;
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ReservasApiResponseDto<ReservaDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<ReservaDto?> CrearReservaAsync(
        CrearReservaRequestDto request, string jwtToken)
    {
        const string endpoint = "api/v1/reservas";

        _logger.LogInformation(
            "[Bus->ReservasF] CrearReserva. " +
            "IdCliente={IdCliente} IdVuelo={IdVuelo} Pasajeros={Pasajeros}",
            request.IdCliente, request.IdVuelo, request.Detalles.Count);

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
                "[Bus->ReservasF] Error de conexión en CrearReserva. " +
                "IdCliente={IdCliente} IdVuelo={IdVuelo}",
                request.IdCliente, request.IdVuelo);
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->ReservasF] CrearReserva fallido. " +
                "StatusCode={StatusCode} Body={Body}",
                (int)response.StatusCode, body);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<ReservasApiResponseDto<ReservaDto>>(body, _jsonOptions);

        if (apiResponse?.Success == true)
        {
            _logger.LogInformation(
                "[Bus->ReservasF] CrearReserva exitoso. " +
                "IdReserva={IdReserva} CodigoReserva={CodigoReserva}",
                apiResponse.Data?.IdReserva,
                apiResponse.Data?.CodigoReserva);
        }

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<ReservaDto?> PagarReservaAsync(
        int idReserva,
        PagarReservaRequestDto request,
        string jwtToken)
    {
        var endpoint = $"api/v1/reservas/{idReserva}/pagar";

        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);  // ← enviar request completo

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->ReservasF] PagarReserva fallido. " +
                "StatusCode={StatusCode} IdReserva={IdReserva} Body={Body}",
                (int)response.StatusCode, idReserva, body);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<ReservasApiResponseDto<ReservaDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<ReservaDto?> CancelarReservaAsync(
        int idReserva, string motivo, string jwtToken)
    {
        var endpoint = $"api/v1/reservas/{idReserva}/estado";

        _logger.LogInformation(
            "[Bus->ReservasF] CancelarReserva. IdReserva={IdReserva}",
            idReserva);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(new
        {
            estadoReserva = "CAN",
            motivoCancelacion = motivo
        });

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(requestMessage);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "[Bus->ReservasF] Error de conexión en CancelarReserva. " +
                "IdReserva={IdReserva}", idReserva);
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "[Bus->ReservasF] CancelarReserva fallido. " +
                "StatusCode={StatusCode} IdReserva={IdReserva}",
                (int)response.StatusCode, idReserva);
            return null;
        }

        var apiResponse = JsonSerializer
            .Deserialize<ReservasApiResponseDto<ReservaDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    // ── BOLETOS ──────────────────────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<BoletoDto?> GetBoletoByIdAsync(int idBoleto, string jwtToken)
    {
        var endpoint = $"api/v1/boletos/{idBoleto}";

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ApiResponseDto<BoletoDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<List<BoletoDto>> GetBoletosByReservaAsync(
        int idReserva, string jwtToken)
    {
        var endpoint = $"api/v1/boletos?idReserva={idReserva}";

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        if (!response.IsSuccessStatusCode) return [];
        /*
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ReservasApiResponseDto<List<BoletoDto>>>(body, _jsonOptions);
        */
        var body = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("[DEBUG] Boletos body: {Body}", body); // ← agrega esto
        var apiResponse = JsonSerializer
            .Deserialize<ApiResponseDto<PaginadoDto<BoletoDto>>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data?.Items ?? [] : [];
    }

    // ── FACTURAS ─────────────────────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<FacturaDto?> GetFacturaByIdAsync(int idFactura, string jwtToken)
    {
        var endpoint = $"api/v1/facturas/{idFactura}";

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ReservasApiResponseDto<FacturaDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <inheritdoc />
    public async Task<FacturaDto?> GetFacturaByReservaAsync(
        int idReserva, string jwtToken)
    {
        var endpoint = $"api/v1/facturas?idReserva={idReserva}";

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ReservasApiResponseDto<FacturaDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    // ── EQUIPAJE ─────────────────────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<List<EquipajeDto>> GetEquipajeByBoletoAsync(
       int idBoleto, string jwtToken)
    {
        var endpoint = $"api/v1/boletos/{idBoleto}/equipaje";

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        if (!response.IsSuccessStatusCode) return [];

        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<ApiResponseDto<PaginadoDto<EquipajeDto>>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data?.Items ?? [] : [];
    }

    /// <inheritdoc />
    public async Task<EquipajeDto?> AgregarEquipajeAsync(
        int idBoleto,
        AgregarEquipajeRequestDto request,
        string jwtToken)
    {
        var endpoint = $"api/v1/boletos/{idBoleto}/equipaje";

        _logger.LogInformation(
            "[Bus->ReservasF] AgregarEquipaje. IdBoleto={IdBoleto} Tipo={Tipo}",
            idBoleto, request.Tipo);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;

        var apiResponse = JsonSerializer
            .Deserialize<ReservasApiResponseDto<EquipajeDto>>(body, _jsonOptions);

        return apiResponse?.Success == true ? apiResponse.Data : null;
    }
}