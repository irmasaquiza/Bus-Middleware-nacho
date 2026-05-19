using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Portal;

namespace Middleware.Vuelos.Business.Interfaces;

public interface IVuelosOrchestrator
{
    /// <summary>
    /// Busca vuelos disponibles según filtros.
    /// Público — no requiere JWT.
    /// </summary>
    Task<List<VueloResponse>> BuscarVuelosAsync(BuscarVuelosRequest request);

    /// <summary>
    /// Obtiene el detalle completo de un vuelo con sus escalas.
    /// Público — no requiere JWT.
    /// </summary>
    Task<VueloResponse> GetVueloByIdAsync(int idVuelo);

    /// <summary>
    /// Obtiene los asientos disponibles de un vuelo.
    /// Público — no requiere JWT.
    /// </summary>
    Task<List<AsientoResponse>> GetAsientosAsync(int idVuelo);

    /// <summary>
    /// Obtiene las escalas de un vuelo.
    /// Público — no requiere JWT.
    /// </summary>
    Task<List<EscalaResponse>> GetEscalasAsync(int idVuelo);
}
