namespace Middleware.Vuelos.Business.DTOs.Vuelos;

/// <summary>
/// Filtros para buscar vuelos disponibles.
/// </summary>
public class BuscarVuelosRequest
{
    public int? IdAeropuertoOrigen { get; set; }
    public int? IdAeropuertoDestino { get; set; }
    public DateTime? FechaSalida { get; set; }
    public int? CantidadPasajeros { get; set; }
    public string? Clase { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
