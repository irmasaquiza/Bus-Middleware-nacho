using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

// ============================================================
// DTOs que mapean exactamente los contratos REST de MS Clientes.
// Validados contra los endpoints reales del microservicio.
// ============================================================

// ------------------------------------------------------------
// GET /api/v1/clientes/{id_cliente}
// POST /api/v1/clientes
// ------------------------------------------------------------

/// <summary>
/// Representa un cliente devuelto por MS Clientes.
/// </summary>
public class ClienteDto
{
    [JsonPropertyName("idCliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("clienteGuid")]
    public Guid ClienteGuid { get; set; }

    [JsonPropertyName("tipoIdentificacion")]
    public string TipoIdentificacion { get; set; } = null!;

    [JsonPropertyName("numeroIdentificacion")]
    public string NumeroIdentificacion { get; set; } = null!;

    [JsonPropertyName("nombres")]
    public string Nombres { get; set; } = null!;

    [JsonPropertyName("apellidos")]
    public string? Apellidos { get; set; }

    [JsonPropertyName("razonSocial")]
    public string? RazonSocial { get; set; }

    [JsonPropertyName("correo")]
    public string Correo { get; set; } = null!;

    [JsonPropertyName("telefono")]
    public string Telefono { get; set; } = null!;

    [JsonPropertyName("direccion")]
    public string Direccion { get; set; } = null!;

    [JsonPropertyName("idCiudadResidencia")]
    public int IdCiudadResidencia { get; set; }

    [JsonPropertyName("idPaisNacionalidad")]
    public int IdPaisNacionalidad { get; set; }

    [JsonPropertyName("fechaNacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [JsonPropertyName("genero")]
    public string? Genero { get; set; }

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = null!;

    [JsonPropertyName("esEliminado")]
    public bool EsEliminado { get; set; }
}

// ------------------------------------------------------------
// POST /api/v1/clientes — Request de creación (admin)
// ------------------------------------------------------------

/// <summary>
/// Request que el Bus envía a MS Clientes para crear un cliente
/// desde el flujo administrativo.
/// Roles requeridos: ADMINISTRADOR, AEROLINEA
/// </summary>
public class CrearClienteRequestDto
{
    [JsonPropertyName("tipoIdentificacion")]
    public string TipoIdentificacion { get; set; } = null!;

    [JsonPropertyName("numeroIdentificacion")]
    public string NumeroIdentificacion { get; set; } = null!;

    [JsonPropertyName("nombres")]
    public string Nombres { get; set; } = null!;

    [JsonPropertyName("apellidos")]
    public string? Apellidos { get; set; }

    [JsonPropertyName("razonSocial")]
    public string? RazonSocial { get; set; }

    [JsonPropertyName("correo")]
    public string Correo { get; set; } = null!;

    [JsonPropertyName("telefono")]
    public string Telefono { get; set; } = null!;

    [JsonPropertyName("direccion")]
    public string Direccion { get; set; } = null!;

    [JsonPropertyName("idCiudadResidencia")]
    public int IdCiudadResidencia { get; set; }

    [JsonPropertyName("idPaisNacionalidad")]
    public int IdPaisNacionalidad { get; set; }

    [JsonPropertyName("fechaNacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [JsonPropertyName("genero")]
    public string? Genero { get; set; }

    [JsonPropertyName("username")]       // ✅ agregar
    public string Username { get; set; } = null!;

    [JsonPropertyName("password")]       // ✅ agregar
    public string Password { get; set; } = null!;
}

// ------------------------------------------------------------
// GET /api/v1/pasajeros/{id_pasajero}
// POST /api/v1/pasajeros
// ------------------------------------------------------------

/// <summary>
/// Representa un pasajero devuelto por MS Clientes.
/// </summary>
public class PasajeroDto
{
    [JsonPropertyName("idPasajero")]
    public int IdPasajero { get; set; }

    [JsonPropertyName("idCliente")]
    public int? IdCliente { get; set; }

    [JsonPropertyName("nombrePasajero")]
    public string NombrePasajero { get; set; } = null!;

    [JsonPropertyName("apellidoPasajero")]
    public string ApellidoPasajero { get; set; } = null!;

    [JsonPropertyName("tipoDocumentoPasajero")]
    public string TipoDocumentoPasajero { get; set; } = null!;

    [JsonPropertyName("numeroDocumentoPasajero")]
    public string NumeroDocumentoPasajero { get; set; } = null!;

    [JsonPropertyName("fechaNacimientoPasajero")]
    public DateTime? FechaNacimientoPasajero { get; set; }

    [JsonPropertyName("idPaisNacionalidad")]
    public int? IdPaisNacionalidad { get; set; }

    [JsonPropertyName("emailContactoPasajero")]
    public string? EmailContactoPasajero { get; set; }

    [JsonPropertyName("telefonoContactoPasajero")]
    public string? TelefonoContactoPasajero { get; set; }

    [JsonPropertyName("generoPasajero")]
    public string? GeneroPasajero { get; set; }

    [JsonPropertyName("requiereAsistencia")]
    public bool RequiereAsistencia { get; set; }

    [JsonPropertyName("observacionesPasajero")]
    public string? ObservacionesPasajero { get; set; }

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = null!;

    [JsonPropertyName("esEliminado")]
    public bool EsEliminado { get; set; }
}

/// <summary>
/// Request para crear un pasajero.
/// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
/// </summary>
public class CrearPasajeroRequestDto
{
    [JsonPropertyName("idCliente")]
    public int? IdCliente { get; set; }

    [JsonPropertyName("nombrePasajero")]
    public string NombrePasajero { get; set; } = null!;

    [JsonPropertyName("apellidoPasajero")]
    public string ApellidoPasajero { get; set; } = null!;

    [JsonPropertyName("tipoDocumentoPasajero")]
    public string TipoDocumentoPasajero { get; set; } = null!;

    [JsonPropertyName("numeroDocumentoPasajero")]
    public string NumeroDocumentoPasajero { get; set; } = null!;

    [JsonPropertyName("fechaNacimientoPasajero")]
    public DateTime? FechaNacimientoPasajero { get; set; }

    [JsonPropertyName("idPaisNacionalidad")]
    public int? IdPaisNacionalidad { get; set; }

    [JsonPropertyName("emailContactoPasajero")]
    public string? EmailContactoPasajero { get; set; }

    [JsonPropertyName("telefonoContactoPasajero")]
    public string? TelefonoContactoPasajero { get; set; }

    [JsonPropertyName("generoPasajero")]
    public string? GeneroPasajero { get; set; }

    [JsonPropertyName("requiereAsistencia")]
    public bool RequiereAsistencia { get; set; }

    [JsonPropertyName("observacionesPasajero")]
    public string? ObservacionesPasajero { get; set; }
}

/// <summary>
/// Wrapper genérico de respuesta de MS Clientes.
/// </summary>
public class ClientesApiResponseDto<T>
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