namespace Middleware.Vuelos.Business.DTOs.Portal;

public class BoletoPortalResponse
{
    public int IdBoleto { get; set; }
    public string CodigoBoleto { get; set; } = null!;
    public int IdVuelo { get; set; }
    public string NumeroVuelo { get; set; } = null!;
    public string NumeroAsiento { get; set; } = null!;
    public string Clase { get; set; } = null!;
    public decimal PrecioFinal { get; set; }
    public string EstadoBoleto { get; set; } = null!;
    public DateTime FechaEmision { get; set; }
    public string CodigoReserva { get; set; } = null!;
}