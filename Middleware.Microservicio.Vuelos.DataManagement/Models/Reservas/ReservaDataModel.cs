namespace Middleware.Vuelos.DataManagement.Models.Reservas;

public class ReservaDataModel
{
    public int IdReserva { get; set; }
    public Guid GuidReserva { get; set; }
    public string CodigoReserva { get; set; } = null!;
    public int IdCliente { get; set; }
    public int IdVuelo { get; set; }
    public DateTime FechaReservaUtc { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal SubtotalReserva { get; set; }
    public decimal ValorIva { get; set; }
    public decimal TotalReserva { get; set; }
    public string OrigenCanalReserva { get; set; } = null!;
    public string EstadoReserva { get; set; } = null!;
    public DateTime? FechaConfirmacionUtc { get; set; }
    public DateTime? FechaCancelacionUtc { get; set; }
    public string? MotivoCancelacion { get; set; }
    public string? ContactoEmail { get; set; }
    public string? ContactoTelefono { get; set; }
    public string? Observaciones { get; set; }
    public bool EsEliminado { get; set; }
    public List<ReservaDetalleDataModel> Detalles { get; set; } = [];

    public bool EsPendiente => EstadoReserva == "PEN";
    public bool EsConfirmada => EstadoReserva == "CON";
    public bool EsEmitida => EstadoReserva == "EMI";
    public bool EstaCancelada => EstadoReserva == "CAN";
    public bool EsPagable => EstadoReserva is "PEN" or "CON";
}