namespace Middleware.Vuelos.Business.DTOs.Portal;

public class ReservaDetallePortalResponse
{
    public int IdDetalle { get; set; }
    public int IdPasajero { get; set; }
    public int IdAsiento { get; set; }
    public string NumeroAsiento { get; set; } = null!;
    public string Clase { get; set; } = null!;
    public decimal TotalLinea { get; set; }
    public string EstadoDetalle { get; set; } = null!;
}