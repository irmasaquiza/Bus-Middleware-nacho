using Middleware.Vuelos.DataAccess.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Clients;

public partial class ReservasFClient
{
    // ── Reservas Admin ────────────────────────────────────────────────────────

    public async Task<ReservasFPagedDto<ReservaDto>?> GetReservasPagedAsync(
        int? idCliente, int? idVuelo, string? codigoReserva,
        string? estadoReserva, int page, int pageSize, string jwtToken)
    {
        var query = $"api/v1/reservas?page={page}&page_size={pageSize}";
        if (idCliente.HasValue) query += $"&id_cliente={idCliente}";
        if (idVuelo.HasValue) query += $"&id_vuelo={idVuelo}";
        if (!string.IsNullOrWhiteSpace(codigoReserva)) query += $"&codigo_reserva={codigoReserva}";
        if (!string.IsNullOrWhiteSpace(estadoReserva)) query += $"&estado_reserva={estadoReserva}";

        using var request = new HttpRequestMessage(HttpMethod.Get, query);
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
            .Deserialize<ReservasApiResponseDto<ReservasFPagedDto<ReservaDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<ReservaDto?> CambiarEstadoReservaAsync(
        int idReserva, string estadoReserva,
        string? motivoCancelacion, string jwtToken)
    {
        var endpoint = $"api/v1/reservas/{idReserva}/estado";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(new
        {
            estadoReserva,
            motivoCancelacion
        });

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
            .Deserialize<ReservasApiResponseDto<ReservaDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    // ── Boletos Admin ─────────────────────────────────────────────────────────

    public async Task<ReservasFPagedDto<BoletoDto>?> GetBoletosPagedAsync(
        int? idReserva, int? idVuelo, string? codigoBoleto,
        string? estadoBoleto, int page, int pageSize, string jwtToken)
    {
        var query = $"api/v1/boletos?page={page}&page_size={pageSize}";
        if (idReserva.HasValue) query += $"&id_reserva={idReserva}";
        if (idVuelo.HasValue) query += $"&id_vuelo={idVuelo}";
        if (!string.IsNullOrWhiteSpace(codigoBoleto)) query += $"&codigo_boleto={codigoBoleto}";
        if (!string.IsNullOrWhiteSpace(estadoBoleto)) query += $"&estado_boleto={estadoBoleto}";

        using var request = new HttpRequestMessage(HttpMethod.Get, query);
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
            .Deserialize<ReservasApiResponseDto<ReservasFPagedDto<BoletoDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<BoletoDto?> CrearBoletoAsync(
        CrearBoletoRequestDto request, string jwtToken)
    {
        const string endpoint = "api/v1/boletos";
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
            .Deserialize<ReservasApiResponseDto<BoletoDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<BoletoDto?> CambiarEstadoBoletoAsync(
        int idBoleto, string estadoBoleto, string jwtToken)
    {
        var endpoint = $"api/v1/boletos/{idBoleto}/estado";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(new { estadoBoleto });

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
            .Deserialize<ReservasApiResponseDto<BoletoDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> EliminarBoletoAsync(int idBoleto, string jwtToken)
    {
        var endpoint = $"api/v1/boletos/{idBoleto}";
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }
        return response.IsSuccessStatusCode;
    }

    // ── Facturas Admin ────────────────────────────────────────────────────────

    public async Task<ReservasFPagedDto<FacturaDto>?> GetFacturasPagedAsync(
        int? idCliente, int? idReserva, string? numeroFactura,
        string? estadoFactura, int page, int pageSize, string jwtToken)
    {
        var query = $"api/v1/facturas?page={page}&page_size={pageSize}";
        if (idCliente.HasValue) query += $"&id_cliente={idCliente}";
        if (idReserva.HasValue) query += $"&id_reserva={idReserva}";
        if (!string.IsNullOrWhiteSpace(numeroFactura)) query += $"&numero_factura={numeroFactura}";
        if (!string.IsNullOrWhiteSpace(estadoFactura)) query += $"&estado_factura={estadoFactura}";

        using var request = new HttpRequestMessage(HttpMethod.Get, query);
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
            .Deserialize<ReservasApiResponseDto<ReservasFPagedDto<FacturaDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<FacturaDto?> CrearFacturaAsync(
        CrearFacturaRequestDto request, string jwtToken)
    {
        const string endpoint = "api/v1/facturas";
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
            .Deserialize<ReservasApiResponseDto<FacturaDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<FacturaDto?> AnularFacturaAsync(int idFactura, string jwtToken)
    {
        var endpoint = $"api/v1/facturas/{idFactura}/anular";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

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
            .Deserialize<ReservasApiResponseDto<FacturaDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<FacturaDto?> AprobarFacturaAsync(int idFactura, string jwtToken)
    {
        var endpoint = $"api/v1/facturas/{idFactura}/aprobar";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

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
            .Deserialize<ReservasApiResponseDto<FacturaDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<FacturaDto?> PagarFacturaAsync(int idFactura, string jwtToken)
    {
        var endpoint = $"api/v1/facturas/{idFactura}/pagar";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

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
            .Deserialize<ReservasApiResponseDto<FacturaDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    public async Task<bool> EliminarFacturaAsync(int idFactura, string jwtToken)
    {
        var endpoint = $"api/v1/facturas/{idFactura}";
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }
        return response.IsSuccessStatusCode;
    }

    // ── Equipaje Admin ────────────────────────────────────────────────────────

    public async Task<EquipajeDto?> CambiarEstadoEquipajeAsync(
        int idBoleto, int idEquipaje, string estadoEquipaje, string jwtToken)
    {
        var endpoint = $"api/v1/boletos/{idBoleto}/equipaje/{idEquipaje}/estado";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Patch, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(new { estadoEquipaje });

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

    public async Task<bool> EliminarEquipajeAsync(
        int idBoleto, int idEquipaje, string jwtToken)
    {
        var endpoint = $"api/v1/boletos/{idBoleto}/equipaje/{idEquipaje}";
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(request); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS ReservasF.", ex);
        }
        return response.IsSuccessStatusCode;
    }
}

// ── DTOs nuevos de ReservasF ──────────────────────────────────────────────────
