namespace Middleware.Vuelos.Business.DTOs.Vuelos;

/// <summary>
/// Filtros para buscar vuelos disponibles.
/// </summary>
/// <summary>
/// Respuesta de escala.
/// </summary>

/// <summary>
/// Respuesta de asiento.
/// </summary>
public class AsientoResponse
{
    public int IdAsiento { get; set; }
    public int IdVuelo { get; set; }
    public string NumeroAsiento { get; set; } = null!;
    public string Clase { get; set; } = null!;
    public bool Disponible { get; set; }
    public decimal PrecioExtra { get; set; }
    public string? Posicion { get; set; }
    public string Estado { get; set; } = null!;
}