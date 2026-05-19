namespace Middleware.Vuelos.Business.DTOs.Reservas;

public class ReservaResponse
{
    public int IdReserva { get; set; }
    public Guid GuidReserva { get; set; }
    public string CodigoReserva { get; set; } = null!;
    public int IdCliente { get; set; }
    public int IdVuelo { get; set; }
    public DateTime FechaReservaUtc { get; set; }
    public decimal SubtotalReserva { get; set; }
    public decimal ValorIva { get; set; }
    public decimal TotalReserva { get; set; }
    public string EstadoReserva { get; set; } = null!;
    public string? ContactoEmail { get; set; }
    public string? ContactoTelefono { get; set; }
    public string? Observaciones { get; set; }
    public List<ReservaDetalleResponse> Detalles { get; set; } = [];
}