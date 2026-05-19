namespace Middleware.Vuelos.Business.DTOs.Aeropuertos;

/// <summary>
/// Respuesta de aeropuerto que el Bus devuelve al frontend.
/// </summary>
public class AeropuertoResponse
{
    public int IdAeropuerto { get; set; }
    public string CodigoIata { get; set; } = null!;
    public string? CodigoIcao { get; set; }
    public string Nombre { get; set; } = null!;
    public int? IdCiudad { get; set; }
    public int IdPais { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string Estado { get; set; } = null!;
}