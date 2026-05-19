namespace Middleware.Vuelos.Business.DTOs.Portal;

public class FacturaPortalResponse
{
    public int IdFactura { get; set; }
    public string NumeroFactura { get; set; } = null!;
    public string CodigoReserva { get; set; } = null!;
    public DateTime FechaEmision { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal Total { get; set; }
    public string EstadoFactura { get; set; } = null!;
}