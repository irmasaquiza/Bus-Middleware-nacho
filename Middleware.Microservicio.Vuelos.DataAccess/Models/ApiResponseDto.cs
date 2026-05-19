using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

/// <summary>
/// Envoltorio genérico de respuesta que usan todos los microservicios.
/// Reutilizable para cualquier MS que devuelva ApiResponse<T>.
/// </summary>
public class ApiResponseDto<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = [];
}