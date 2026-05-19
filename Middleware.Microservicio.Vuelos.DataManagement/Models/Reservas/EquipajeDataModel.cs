namespace Middleware.Vuelos.DataManagement.Models.Reservas;

public class EquipajeDataModel
{
    public int IdEquipaje { get; set; }
    public int IdBoleto { get; set; }
    public string Tipo { get; set; } = null!;
    public decimal PesoKg { get; set; }
    public string? DescripcionEquipaje { get; set; }
    public decimal PrecioExtra { get; set; }
    public string? DimensionesCm { get; set; }
    public string NumeroEtiqueta { get; set; } = null!;
    public string EstadoEquipaje { get; set; } = null!;
    public bool EsEliminado { get; set; }
}