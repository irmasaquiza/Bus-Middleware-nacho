namespace Middleware.Vuelos.Business.DTOs.Reservas;

public class CrearReservaRequest
{
    /// <summary>
    /// Requerido para ADMINISTRADOR y AEROLINEA.
    /// Para CLIENTE se ignora — viene del JWT automáticamente.
    /// </summary>
    public int? IdCliente { get; set; }

    public int IdVuelo { get; set; }
    public string? ContactoEmail { get; set; }
    public string? ContactoTelefono { get; set; }
    public string? Observaciones { get; set; }

    /// <summary>
    /// Lista de pasajeros con su asiento asignado.
    /// </summary>
    public List<ReservaDetalleRequest> Detalles { get; set; } = [];
}