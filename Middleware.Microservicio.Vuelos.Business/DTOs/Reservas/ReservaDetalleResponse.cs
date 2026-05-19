namespace Middleware.Vuelos.Business.DTOs.Reservas;

public class ReservaDetalleResponse
{
    public int IdDetalle { get; set; }
    public int IdPasajero { get; set; }
    public int IdAsiento { get; set; }
    public decimal SubtotalLinea { get; set; }
    public decimal TotalLinea { get; set; }
    public string EstadoDetalle { get; set; } = null!;
}