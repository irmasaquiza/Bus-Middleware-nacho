namespace Middleware.Vuelos.DataManagement.Models.Geografia;

public class CiudadDataModel
{
    public int IdCiudad { get; set; }
    public int IdPais { get; set; }
    public string Nombre { get; set; } = null!;
    public string? CodigoPostal { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string Estado { get; set; } = null!;
    public bool Eliminado { get; set; }

    /// <summary>
    /// Calculado localmente — no requiere llamada adicional.
    /// </summary>
    public bool EsActiva => !Eliminado && Estado == "ACT";
}