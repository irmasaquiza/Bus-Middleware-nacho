using Middleware.Vuelos.DataAccess.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Clients;

public partial class VuelosClient
{
    // ── Booking endpoints ─────────────────────────────────────────────────────

    /// <summary>
    /// Busca vuelos disponibles para booking.
    /// GET /api/v1/booking/vuelos/buscar
    /// Público.
    /// </summary>
    public async Task<VuelosAdminPagedDto<VueloDto>?> BookingBuscarVuelosAsync(
        string? codigoIataOrigen, string? codigoIataDestino,
        int? idAeropuertoOrigen, int? idAeropuertoDestino,
        DateTime? fechaSalida, int? cantidadPasajeros,
        string? clase, int page, int pageSize)
    {
        var query = $"api/v1/booking/vuelos/buscar?Page={page}&Limit={pageSize}";

        if (!string.IsNullOrWhiteSpace(codigoIataOrigen)) query += $"&Origen={codigoIataOrigen}";
        if (!string.IsNullOrWhiteSpace(codigoIataDestino)) query += $"&Destino={codigoIataDestino}";
        if (fechaSalida.HasValue) query += $"&Fecha={fechaSalida:yyyy-MM-dd}";
        if (!string.IsNullOrWhiteSpace(clase)) query += $"&Clase={clase}";
        if (cantidadPasajeros.HasValue) query += $"&Pasajeros={cantidadPasajeros}";

        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(query); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VuelosBookingResponseDto>>(body, _jsonOptions);

        var bookingResponse = apiResponse?.Data;
        if (bookingResponse?.Data is null) return null;

        // Mapear BookingVueloItemDto → VueloDto correctamente
        var items = bookingResponse.Data.Select(v => new VueloDto
        {
            IdVuelo = v.IdVuelo,
            NumeroVuelo = v.NumeroVuelo,
            Origen = v.Origen,
            Destino = v.Destino,
            FechaHoraSalida = v.FechaHoraSalida,
            FechaHoraLlegada = v.FechaHoraLlegada,
            DuracionMin = v.DuracionMin,
            PrecioBase = v.PrecioBase,
            CapacidadTotal = v.AsientosDisponibles, // ← mapear asientosDisponibles
            AsientosDisponibles = v.AsientosDisponibles, // ← agregar
            EstadoVuelo = v.EstadoVuelo ?? string.Empty,
            Estado = null,
            Eliminado = false
        }).ToList();

        return new VuelosAdminPagedDto<VueloDto>
        {
            Items = items,
            TotalRegistros = bookingResponse.Meta?.Total ?? 0,
            PaginaActual = bookingResponse.Meta?.Page ?? 1,
            TotalPaginas = (bookingResponse.Meta?.Total ?? 0) / (pageSize == 0 ? 20 : pageSize)
        };
    }

    /// <summary>
    /// Obtiene detalle de vuelo para booking.
    /// GET /api/v1/booking/vuelos/{id_vuelo}
    /// Público.
    /// </summary>
    public async Task<VueloDto?> BookingGetVueloByIdAsync(int idVuelo)
    {
        var endpoint = $"api/v1/booking/vuelos/{idVuelo}";
        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(endpoint); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<VueloDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }

    /// <summary>
    /// Obtiene escalas de vuelo para booking.
    /// GET /api/v1/booking/vuelos/{id_vuelo}/escalas
    /// Público.
    /// </summary>
    public async Task<List<EscalaDto>> BookingGetEscalasAsync(int idVuelo)
    {
        var endpoint = $"api/v1/booking/vuelos/{idVuelo}/escalas";
        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(endpoint); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return [];
        var body = await response.Content.ReadAsStringAsync();

        // MS Vuelos devuelve {idVuelo, numeroVuelo, numEscalas, escalas}
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<BookingEscalasResponseDto>>(
                body, _jsonOptions);
        return apiResponse?.Data?.Escalas ?? [];
    }

    /// <summary>
    /// Obtiene asientos disponibles para booking.
    /// GET /api/v1/booking/vuelos/{id_vuelo}/asientos
    /// Público.
    /// </summary>
    public async Task<List<AsientoDto>> BookingGetAsientosAsync(int idVuelo)
    {
        var endpoint = $"api/v1/booking/vuelos/{idVuelo}/asientos";
        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(endpoint); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return [];
        var body = await response.Content.ReadAsStringAsync();

        // MS Vuelos devuelve {idVuelo, numeroVuelo, resumen, asientos}
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<BookingAsientosResponseDto>>(
                body, _jsonOptions);
        return apiResponse?.Data?.Asientos ?? [];
    }

    /// <summary>
    /// Busca aeropuertos para booking.
    /// GET /api/v1/booking/aeropuertos
    /// Público.
    /// </summary>
    public async Task<List<AeropuertoDto>> BookingBuscarAeropuertosAsync(
        string? nombre, int? idPais, int limit)
    {
        var query = $"api/v1/booking/aeropuertos?limit={limit}";
        if (!string.IsNullOrWhiteSpace(nombre)) query += $"&nombre={nombre}";
        if (idPais.HasValue) query += $"&idPais={idPais}";

        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(query); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        if (!response.IsSuccessStatusCode) return [];
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<List<AeropuertoDto>>>(body, _jsonOptions);
        return apiResponse?.Data ?? [];
    }

    /// <summary>
    /// Inicia sesión de redirect a aerolínea.
    /// POST /api/v1/booking/vuelos/sesion-redirect
    /// Rol: BOOKING
    /// </summary>
    public async Task<BookingSessionRedirectResponseDto?> BookingSessionRedirectAsync(
        BookingSessionRedirectRequestDto request, string jwtToken)
    {
        const string endpoint = "api/v1/booking/vuelos/sesion-redirect";
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Vuelos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<VuelosApiResponseDto<BookingSessionRedirectResponseDto>>(
                body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }
}

// ── DTOs de Booking ───────────────────────────────────────────────────────────
public class BookingVueloItemDto
{
    [JsonPropertyName("idVuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("numeroVuelo")]
    public string NumeroVuelo { get; set; } = null!;

    [JsonPropertyName("origen")]
    public AeropuertoCortoDto? Origen { get; set; }

    [JsonPropertyName("destino")]
    public AeropuertoCortoDto? Destino { get; set; }

    [JsonPropertyName("fechaHoraSalida")]
    public DateTime FechaHoraSalida { get; set; }

    [JsonPropertyName("fechaHoraLlegada")]
    public DateTime FechaHoraLlegada { get; set; }

    [JsonPropertyName("duracionMin")]
    public int DuracionMin { get; set; }

    [JsonPropertyName("precioBase")]
    public decimal PrecioBase { get; set; }

    [JsonPropertyName("asientosDisponibles")]
    public int AsientosDisponibles { get; set; }

    [JsonPropertyName("estadoVuelo")]
    public string? EstadoVuelo { get; set; }


}

public class BookingEscalasResponseDto
{
    [JsonPropertyName("idVuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("numeroVuelo")]
    public string NumeroVuelo { get; set; } = null!;

    [JsonPropertyName("numEscalas")]
    public int NumEscalas { get; set; }

    [JsonPropertyName("escalas")]
    public List<EscalaDto>? Escalas { get; set; }
}