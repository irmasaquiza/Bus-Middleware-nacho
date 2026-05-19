namespace Middleware.Vuelos.Business.DTOs.Vuelos;

/// <summary>
/// Filtros para buscar vuelos disponibles.
/// </summary>


/// <summary>
/// Respuesta de vuelo que el Bus devuelve al frontend.
/// </summary>
public class VueloResponse
{
    public int IdVuelo { get; set; }
    public int IdAeropuertoOrigen { get; set; }
    public int IdAeropuertoDestino { get; set; }
    public string NumeroVuelo { get; set; } = null!;
    public DateTime FechaHoraSalida { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public int DuracionMin { get; set; }
    public decimal PrecioBase { get; set; }
    public int CapacidadTotal { get; set; }
    public string EstadoVuelo { get; set; } = null!;
    public List<EscalaResponse> Escalas { get; set; } = [];
}
