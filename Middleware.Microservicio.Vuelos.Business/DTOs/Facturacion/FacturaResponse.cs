namespace Middleware.Vuelos.Business.DTOs.Facturacion;

/// <summary>
/// Respuesta de factura que el Bus devuelve al frontend.
/// </summary>
public class FacturaResponse
{
    public int IdFactura { get; set; }
    public Guid GuidFactura { get; set; }
    public int IdCliente { get; set; }
    public int IdReserva { get; set; }
    public string NumeroFactura { get; set; } = null!;
    public DateTime FechaEmision { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal CargoServicio { get; set; }
    public decimal Total { get; set; }
    public string EstadoFactura { get; set; } = null!;
    public string? ObservacionesFactura { get; set; }
}
