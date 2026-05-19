using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

// ============================================================
// DTOs que mapean exactamente los contratos REST del MS Seguridad.
// Validados contra el código real del microservicio.
// ============================================================

// ------------------------------------------------------------
// POST /api/v1/auth/login
// ------------------------------------------------------------

/// <summary>
/// Request exacto de login contra MS Seguridad.
/// Solo recibe username y password, nada más.
/// </summary>
public class LoginRequestDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}

/// <summary>
/// Response real de login de MS Seguridad, desenvuelta de ApiResponse.
/// No incluye id_usuario ni id_cliente. El id_cliente solo viaja en el JWT.
/// </summary>
public class LoginResponseDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;

    [JsonPropertyName("usuario")]
    public string Usuario { get; set; } = null!;

    /// <summary>Fecha UTC de expiración del token.</summary>
    [JsonPropertyName("expiracion")]
    public DateTime Expiracion { get; set; }

    /// <summary>Lista de roles activos del usuario autenticado.</summary>
    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } = [];
}

// ------------------------------------------------------------
// POST /api/v1/internal/seguridad/users/create-for-client
// Endpoint nuevo creado para el Bus.
// ------------------------------------------------------------

/// <summary>
/// Request que el Bus envía a MS Seguridad para crear usuario
/// con IdCliente real vinculado. Resuelve el problema de IdCliente = 0.
/// </summary>
public class CreateUserForClientRequestDto
{
    [JsonPropertyName("id_cliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("correo")]
    public string Correo { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;

    [JsonPropertyName("correlation_id")]
    public string? CorrelationId { get; set; }
}

/// <summary>
/// Response de MS Seguridad tras crear el usuario con IdCliente vinculado.
/// </summary>
public class CreateUserForClientResponseDto
{
    [JsonPropertyName("idUsuario")]
    public int IdUsuario { get; set; }

    [JsonPropertyName("usuarioGuid")]
    public Guid UsuarioGuid { get; set; }

    [JsonPropertyName("idCliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("rolAsignado")]
    public string RolAsignado { get; set; } = null!;

    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }
}

// ------------------------------------------------------------
// Wrapper genérico ApiResponse<T> que MS Seguridad usa en todas
// sus respuestas. Necesario para deserializar correctamente.
// ------------------------------------------------------------

/// <summary>
/// Envoltorio estándar de respuesta de MS Seguridad.
/// Todos los endpoints devuelven esta estructura.
/// </summary>
public class SeguridadApiResponseDto<T>
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