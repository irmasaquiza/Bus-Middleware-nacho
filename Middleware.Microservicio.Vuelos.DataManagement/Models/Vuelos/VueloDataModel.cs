namespace Middleware.Vuelos.DataManagement.Models.Vuelos;

public class VueloDataModel
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
    public string Estado { get; set; } = null!;
    public bool Eliminado { get; set; }

    public bool EsOperable =>
        !Eliminado &&
        Estado is "ACT" or "ACTIVO" &&
        EstadoVuelo is "PROGRAMADO" or "DEMORADO";

    public bool EstaCancelado => EstadoVuelo == "CANCELADO";
}