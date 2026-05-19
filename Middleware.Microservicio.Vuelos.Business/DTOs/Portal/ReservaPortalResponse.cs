namespace Middleware.Vuelos.Business.DTOs.Portal;

public class ReservaPortalResponse
{
    public int IdReserva { get; set; }
    public string CodigoReserva { get; set; } = null!;
    public int IdVuelo { get; set; }
    public string NumeroVuelo { get; set; } = null!;
    public DateTime FechaReservaUtc { get; set; }
    public decimal TotalReserva { get; set; }
    public string EstadoReserva { get; set; } = null!;
    public string? ContactoEmail { get; set; }
    public List<ReservaDetallePortalResponse> Detalles { get; set; } = [];
}