namespace Middleware.Vuelos.DataManagement.Models.Geografia;

public class PaisDataModel
{
    public int IdPais { get; set; }
    public string CodigoIso2 { get; set; } = null!;
    public string? CodigoIso3 { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Continente { get; set; }
    public string Estado { get; set; } = null!;
    public bool Eliminado { get; set; }

    /// <summary>
    /// Calculado localmente — no requiere llamada adicional.
    /// </summary>
    public bool EsActivo => !Eliminado && Estado == "ACT";
}