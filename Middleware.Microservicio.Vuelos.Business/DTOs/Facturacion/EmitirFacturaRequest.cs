namespace Middleware.Vuelos.Business.DTOs.Facturacion;

/// <summary>
/// Request para emitir una factura manualmente.
/// Generalmente la factura se genera automáticamente al pagar.
/// </summary>
public class EmitirFacturaRequest
{
    public int IdReserva { get; set; }
    public string? ObservacionesFactura { get; set; }
}