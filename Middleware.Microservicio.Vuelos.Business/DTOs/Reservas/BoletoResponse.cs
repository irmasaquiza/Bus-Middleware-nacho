namespace Middleware.Vuelos.Business.DTOs.Reservas;

public class BoletoResponse
{
    public int IdBoleto { get; set; }
    public int IdReserva { get; set; }
    public int IdVuelo { get; set; }
    public int IdAsiento { get; set; }
    public string CodigoBoleto { get; set; } = null!;
    public string Clase { get; set; } = null!;
    public decimal PrecioVueloBase { get; set; }
    public decimal PrecioAsientoExtra { get; set; }
    public decimal ImpuestosBoleto { get; set; }
    public decimal CargoEquipaje { get; set; }
    public decimal PrecioFinal { get; set; }
    public string EstadoBoleto { get; set; } = null!;
    public DateTime FechaEmision { get; set; }
}