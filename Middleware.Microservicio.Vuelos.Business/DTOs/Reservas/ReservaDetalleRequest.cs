namespace Middleware.Vuelos.Business.DTOs.Reservas;

public class ReservaDetalleRequest
{
    public int IdPasajero { get; set; }
    public int IdAsiento { get; set; }

    public decimal SubtotalLinea { get; set; }  // ← agregar
    public decimal ValorIvaLinea { get; set; }  // ← agregar
    public decimal TotalLinea { get; set; }     // ← agregar
}