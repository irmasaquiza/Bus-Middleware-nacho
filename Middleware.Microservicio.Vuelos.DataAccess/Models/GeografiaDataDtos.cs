using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

// ============================================================
// DTOs que mapean exactamente los contratos REST de MS Geografía.
// Validados contra los endpoints reales del microservicio.
// ============================================================

// ------------------------------------------------------------
// GET /api/v1/paises/{id_pais}
// GET /api/v1/paises  (paginado)
// ------------------------------------------------------------

/// <summary>
/// Representa un país devuelto por MS Geografía.
/// Mapeado desde la respuesta real del endpoint.
/// </summary>
public class PaisDto
{
    [JsonPropertyName("idPais")]
    public int IdPais { get; set; }

    [JsonPropertyName("codigoIso2")]
    public string CodigoIso2 { get; set; } = null!;

    [JsonPropertyName("codigoIso3")]
    public string? CodigoIso3 { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;

    [JsonPropertyName("continente")]
    public string? Continente { get; set; }

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = null!;

    [JsonPropertyName("eliminado")]
    public bool Eliminado { get; set; }
}

// ------------------------------------------------------------
// GET /api/v1/ciudades/{id_ciudad}
// GET /api/v1/ciudades  (paginado)
// ------------------------------------------------------------

/// <summary>
/// Representa una ciudad devuelta por MS Geografía.
/// </summary>
public class CiudadDto
{
    [JsonPropertyName("idCiudad")]
    public int IdCiudad { get; set; }

    [JsonPropertyName("idPais")]
    public int IdPais { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;

    [JsonPropertyName("codigoPostal")]
    public string? CodigoPostal { get; set; }

    [JsonPropertyName("zonaHoraria")]
    public string? ZonaHoraria { get; set; }

    [JsonPropertyName("latitud")]
    public decimal? Latitud { get; set; }

    [JsonPropertyName("longitud")]
    public decimal? Longitud { get; set; }

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = null!;

    [JsonPropertyName("eliminado")]
    public bool Eliminado { get; set; }
}

// ------------------------------------------------------------
// Wrapper genérico ApiResponse<T> que MS Geografía usa.
// Necesario para deserializar correctamente todas las respuestas.
// ------------------------------------------------------------

/// <summary>
/// Envoltorio estándar de respuesta de MS Geografía.
/// Todos los endpoints devuelven esta estructura.
/// </summary>
public class GeografiaApiResponseDto<T>
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