using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Portal;

namespace Middleware.Vuelos.Business.Interfaces;

public interface IAeropuertosOrchestrator
{
    /// <summary>
    /// Obtiene un aeropuerto por su id.
    /// Público — no requiere JWT.
    /// </summary>
    Task<AeropuertoResponse> GetByIdAsync(int idAeropuerto);

    /// <summary>
    /// Obtiene un aeropuerto por su código IATA.
    /// Público — no requiere JWT.
    /// </summary>
    Task<AeropuertoResponse> GetByCodigoIataAsync(string codigoIata);
}