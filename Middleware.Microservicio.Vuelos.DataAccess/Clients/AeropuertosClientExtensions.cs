using Microsoft.Extensions.Logging;
using Middleware.Vuelos.DataAccess.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Clients;

public partial class AeropuertosClient
{
    public async Task<AeropuertosPagedResponseDto<AeropuertoDto>?> GetPagedAsync(
        string? codigoIata, string? codigoIcao, string? nombre,
        int? idCiudad, int? idPais, string? zonaHoraria,
        string? estado, int page, int pageSize)
    {
        var query = $"api/v1/aeropuertos?page={page}&page_size={pageSize}";
        if (!string.IsNullOrWhiteSpace(codigoIata)) query += $"&codigo_iata={codigoIata}";
        if (!string.IsNullOrWhiteSpace(codigoIcao)) query += $"&codigo_icao={codigoIcao}";
        if (!string.IsNullOrWhiteSpace(nombre)) query += $"&nombre={nombre}";
        if (idCiudad.HasValue) query += $"&id_ciudad={idCiudad}";
        if (idPais.HasValue) query += $"&id_pais={idPais}";
        if (!string.IsNullOrWhiteSpace(estado)) query += $"&estado={estado}";

        HttpResponseMessage response;
        try { response = await _httpClient.GetAsync(query); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Aeropuertos.", ex);
        }

        if (!response.IsSuccessStatusCode) return null;
        var body = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer
            .Deserialize<AeropuertosApiResponseDto<AeropuertosPagedResponseDto<AeropuertoDto>>>(
                body, _jsonOptions);
        return apiResponse?.Data;
    }

    public async Task<AeropuertoDto?> ActualizarAsync(
        int idAeropuerto, ActualizarAeropuertoRequestDto request, string jwtToken)
    {
        var endpoint = $"api/v1/aeropuertos/{idAeropuerto}";
        using var requestMessage = new System.Net.Http.HttpRequestMessage(
            System.Net.Http.HttpMethod.Put, endpoint);
        requestMessage.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
        requestMessage.Content = JsonContent.Create(request);

        HttpResponseMessage response;
        try { response = await _httpClient.SendAsync(requestMessage); }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "No se pudo conectar con MS Aeropuertos.", ex);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) return null;
        var apiResponse = JsonSerializer
            .Deserialize<AeropuertosApiResponseDto<AeropuertoDto>>(body, _jsonOptions);
        return apiResponse?.Success == true ? apiResponse.Data : null;
    }
}
