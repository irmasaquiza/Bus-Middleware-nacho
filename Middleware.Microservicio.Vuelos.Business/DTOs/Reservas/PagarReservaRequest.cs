namespace Middleware.Vuelos.Business.DTOs.Reservas;

public class PagarReservaRequest
{
    public decimal CargoServicio { get; set; }
    public List<PagarReservaEquipajeRequest> Equipaje { get; set; } = [];
}

public class PagarReservaEquipajeRequest
{
    public int IdDetalle { get; set; }
    public string Tipo { get; set; } = null!;
    public decimal PesoKg { get; set; }
    public string? DescripcionEquipaje { get; set; }
}