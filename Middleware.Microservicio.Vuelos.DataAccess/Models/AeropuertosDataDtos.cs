using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

// ============================================================
// DTOs que mapean exactamente los contratos REST de MS Aeropuertos.
// ============================================================

/// <summary>
/// Representa un aeropuerto devuelto por MS Aeropuertos.
/// </summary>
public class AeropuertoDto
{
    [JsonPropertyName("idAeropuerto")]
    public int IdAeropuerto { get; set; }

    [JsonPropertyName("codigoIata")]
    public string CodigoIata { get; set; } = null!;

    [JsonPropertyName("codigoIcao")]
    public string? CodigoIcao { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;

    // Booking devuelve objetos anidados
    [JsonPropertyName("ciudad")]
    public CiudadCortoDto? Ciudad { get; set; }

    [JsonPropertyName("pais")]
    public PaisCortoDto? Pais { get; set; }

    // Admin devuelve IDs directos
    [JsonPropertyName("idCiudad")]
    public int? IdCiudadRaw { get; set; }

    [JsonPropertyName("idPais")]
    public int? IdPaisRaw { get; set; }

    // Computed — SIN [JsonPropertyName]
    [JsonIgnore]
    public int? IdCiudad => IdCiudadRaw ?? (Ciudad?.IdCiudad == 0 ? null : Ciudad?.IdCiudad);

    [JsonIgnore]
    public int IdPais => IdPaisRaw ?? Pais?.IdPais ?? 0;

    [JsonPropertyName("zonaHoraria")]
    public string? ZonaHoraria { get; set; }

    [JsonPropertyName("latitud")]
    public decimal? Latitud { get; set; }

    [JsonPropertyName("longitud")]
    public decimal? Longitud { get; set; }

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [JsonPropertyName("eliminado")]
    public bool Eliminado { get; set; }
}

public class CiudadCortoDto
{
    [JsonPropertyName("idCiudad")]
    public int IdCiudad { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;
}

public class PaisCortoDto
{
    [JsonPropertyName("idPais")]
    public int IdPais { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;
}

public class CiudadIntegrationDto
{
    [JsonPropertyName("idCiudad")]
    public int IdCiudad { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;
}

public class PaisIntegrationDto
{
    [JsonPropertyName("idPais")]
    public int IdPais { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;
}

// ------------------------------------------------------------
// POST /api/v1/aeropuertos — Request de creación
// ------------------------------------------------------------

/// <summary>
/// Request que el Bus envía a MS Aeropuertos para crear un aeropuerto.
/// El Bus ya validó país y ciudad contra MS Geografía antes de llegar aquí.
/// </summary>
public class CrearAeropuertoRequestDto
{
    [JsonPropertyName("codigoIata")]
    public string CodigoIata { get; set; } = null!;

    [JsonPropertyName("codigoIcao")]
    public string? CodigoIcao { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;

    [JsonPropertyName("idCiudad")]
    public int? IdCiudad { get; set; }

    [JsonPropertyName("idPais")]
    public int IdPais { get; set; }

    [JsonPropertyName("zonaHoraria")]
    public string? ZonaHoraria { get; set; }

    [JsonPropertyName("latitud")]
    public decimal? Latitud { get; set; }

    [JsonPropertyName("longitud")]
    public decimal? Longitud { get; set; }
}

/// <summary>
/// Wrapper genérico de respuesta de MS Aeropuertos.
/// </summary>
public class AeropuertosApiResponseDto<T>
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