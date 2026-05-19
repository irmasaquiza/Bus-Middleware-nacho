namespace Middleware.Vuelos.DataManagement.Models.Reservas;

public class ReservaDetalleDataModel
{
    public int IdDetalle { get; set; }
    public int IdReserva { get; set; }
    public int IdPasajero { get; set; }
    public int IdAsiento { get; set; }
    public decimal SubtotalLinea { get; set; }
    public decimal ValorIvaLinea { get; set; }
    public decimal TotalLinea { get; set; }
    public string EstadoDetalle { get; set; } = null!;
    public bool EsEliminado { get; set; }
}