namespace Middleware.Vuelos.Business.DTOs.Seguridad;

/// <summary>
/// Request de login que el frontend envía al Bus.
/// El Bus lo reenvía a MS Seguridad.
/// </summary>
public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

/// <summary>
/// Respuesta de login que el Bus devuelve al frontend.
/// El frontend guarda el token y lo envía en cada request posterior.
/// </summary>
