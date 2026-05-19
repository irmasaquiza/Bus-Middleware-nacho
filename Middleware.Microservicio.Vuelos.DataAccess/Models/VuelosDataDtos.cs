using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

// ============================================================
// DTOs que mapean exactamente los contratos REST de MS Vuelos.
// Endpoints validados contra el microservicio real.
// ============================================================

// ------------------------------------------------------------
// GET /api/v1/vuelos/{id_vuelo}
// POST /api/v1/vuelos
// PUT /api/v1/vuelos/{id_vuelo}
// ------------------------------------------------------------

/// <summary>
/// Representa un vuelo devuelto por MS Vuelos.
/// </summary>
public class VueloDto
{
    [JsonPropertyName("idVuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("origen")]
    public AeropuertoCortoDto? Origen { get; set; }

    [JsonPropertyName("destino")]
    public AeropuertoCortoDto? Destino { get; set; }

    // Admin devuelve IDs directos
    [JsonPropertyName("idAeropuertoOrigen")]
    public int? IdAeropuertoOrigenRaw { get; set; }

    [JsonPropertyName("idAeropuertoDestino")]
    public int? IdAeropuertoDestinoRaw { get; set; }

    // Computed — SIN [JsonPropertyName] para evitar colisión
    [JsonIgnore]
    public int IdAeropuertoOrigen =>
        IdAeropuertoOrigenRaw ?? Origen?.IdAeropuerto ?? 0;

    [JsonIgnore]
    public int IdAeropuertoDestino =>
        IdAeropuertoDestinoRaw ?? Destino?.IdAeropuerto ?? 0;

    [JsonPropertyName("numeroVuelo")]
    public string NumeroVuelo { get; set; } = null!;

    [JsonPropertyName("fechaHoraSalida")]
    public DateTime FechaHoraSalida { get; set; }

    [JsonPropertyName("fechaHoraLlegada")]
    public DateTime FechaHoraLlegada { get; set; }

    [JsonPropertyName("duracionMin")]
    public int DuracionMin { get; set; }

    [JsonPropertyName("precioBase")]
    public decimal PrecioBase { get; set; }

    [JsonPropertyName("capacidadTotal")]
    public int CapacidadTotal { get; set; }

    [JsonPropertyName("estadoVuelo")]
    public string EstadoVuelo { get; set; } = null!;

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [JsonPropertyName("eliminado")]
    public bool Eliminado { get; set; }
}

public class AeropuertoCortoDto
{
    [JsonPropertyName("idAeropuerto")]
    public int IdAeropuerto { get; set; }

    [JsonPropertyName("codigoIata")]
    public string CodigoIata { get; set; } = null!;

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;
}

public class AeropuertoIntegrationDto
{
    [JsonPropertyName("idAeropuerto")]
    public int IdAeropuerto { get; set; }

    [JsonPropertyName("codigoIata")]
    public string CodigoIata { get; set; } = null!;

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = null!;
}

// ------------------------------------------------------------
// GET /api/v1/vuelos/{id_vuelo}/escalas/{id_escala}
// POST /api/v1/vuelos/{id_vuelo}/escalas
// ------------------------------------------------------------

/// <summary>
/// Representa una escala devuelta por MS Vuelos.
/// </summary>
public class EscalaDto
{
    [JsonPropertyName("idEscala")]
    public int IdEscala { get; set; }

    [JsonPropertyName("idVuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("idAeropuerto")]
    public int IdAeropuerto { get; set; }

    [JsonPropertyName("orden")]
    public int Orden { get; set; }

    [JsonPropertyName("aeropuerto")]          // ← agregar
    public AeropuertoCortoDto? Aeropuerto { get; set; }  // ← agregar

    [JsonPropertyName("fechaHoraLlegada")]
    public DateTime FechaHoraLlegada { get; set; }

    [JsonPropertyName("fechaHoraSalida")]
    public DateTime FechaHoraSalida { get; set; }

    [JsonPropertyName("duracionMin")]
    public int DuracionMin { get; set; }

    [JsonPropertyName("tipoEscala")]
    public string? TipoEscala { get; set; }

    [JsonPropertyName("terminal")]
    public string? Terminal { get; set; }

    [JsonPropertyName("puerta")]
    public string? Puerta { get; set; }

    [JsonPropertyName("observaciones")]
    public string? Observaciones { get; set; }

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [JsonPropertyName("eliminado")]
    public bool Eliminado { get; set; }
}
// ------------------------------------------------------------
// GET /api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
// GET /api/v1/vuelos/{id_vuelo}/asientos
// PATCH /api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
// ------------------------------------------------------------

/// <summary>
/// Representa un asiento devuelto por MS Vuelos.
/// </summary>
public class AsientoDto
{
    [JsonPropertyName("idAsiento")]
    public int IdAsiento { get; set; }

    [JsonPropertyName("idVuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("numeroAsiento")]
    public string NumeroAsiento { get; set; } = null!;

    [JsonPropertyName("clase")]
    public string Clase { get; set; } = null!;

    [JsonPropertyName("disponible")]
    public bool Disponible { get; set; }

    [JsonPropertyName("precioExtra")]
    public decimal PrecioExtra { get; set; }

    [JsonPropertyName("posicion")]
    public string? Posicion { get; set; }

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }  // ← nullable

    [JsonPropertyName("eliminado")]
    public bool Eliminado { get; set; }
}

// ------------------------------------------------------------
// PATCH /api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
// Usado por el Bus para marcar asiento como no disponible
// al procesar reservas desde MS ReservasF.
// Roles: ADMINISTRADOR, AEROLINEA
// ------------------------------------------------------------

/// <summary>
/// Request para actualizar disponibilidad de un asiento.
/// El Bus lo usa al coordinar reservas con MS ReservasF.
/// </summary>
public class ActualizarAsientoRequestDto
{
    [JsonPropertyName("disponible")]
    public bool Disponible { get; set; }
}

/// <summary>
/// Wrapper genérico de respuesta de MS Vuelos.
/// </summary>
public class VuelosApiResponseDto<T>
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


public class BookingAsientosResponseDto
{
    [JsonPropertyName("idVuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("numeroVuelo")]
    public string NumeroVuelo { get; set; } = null!;

    [JsonPropertyName("resumen")]
    public BookingAsientosResumenDto? Resumen { get; set; }

    [JsonPropertyName("asientos")]
    public List<AsientoDto>? Asientos { get; set; }
}

public class BookingAsientosResumenDto
{
    [JsonPropertyName("totalAsientos")]
    public int TotalAsientos { get; set; }

    [JsonPropertyName("disponibles")]
    public int Disponibles { get; set; }

    [JsonPropertyName("ocupados")]
    public int Ocupados { get; set; }
}