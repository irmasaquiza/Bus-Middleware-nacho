namespace Middleware.Vuelos.Business.DTOs.Seguridad;

/// <summary>
/// Respuesta de login que el Bus devuelve al frontend.
/// El frontend guarda el token y lo envía en cada request posterior.
/// </summary>
public class LoginResponse
{
    public string Token { get; set; } = null!;
    public string Usuario { get; set; } = null!;
    public DateTime Expiracion { get; set; }
    public List<string> Roles { get; set; } = [];
}