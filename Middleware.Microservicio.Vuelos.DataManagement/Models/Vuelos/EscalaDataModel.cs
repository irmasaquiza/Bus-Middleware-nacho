namespace Middleware.Vuelos.DataManagement.Models.Vuelos;

public class EscalaDataModel
{
    public int IdEscala { get; set; }
    public int IdVuelo { get; set; }
    public int IdAeropuerto { get; set; }
    public int Orden { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public DateTime FechaHoraSalida { get; set; }
    public int DuracionMin { get; set; }
    public string TipoEscala { get; set; } = null!;
    public string? Terminal { get; set; }
    public string? Puerta { get; set; }
    public string? Observaciones { get; set; }
    public string Estado { get; set; } = null!;
    public bool Eliminado { get; set; }
}