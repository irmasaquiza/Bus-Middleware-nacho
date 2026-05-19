namespace Middleware.Vuelos.DataManagement.Models.Aeropuertos;

/// <summary>
/// Modelo interno del Bus que representa un aeropuerto de MS Aeropuertos.
/// Viaja entre DataManagement y Business del Bus.
/// </summary>
public class AeropuertoDataModel
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
    public bool Eliminado { get; set; }

    /// <summary>
    /// Calculado localmente — no requiere llamada adicional.
    /// </summary>
    public bool EsActivo => !Eliminado && Estado == "ACT";
}