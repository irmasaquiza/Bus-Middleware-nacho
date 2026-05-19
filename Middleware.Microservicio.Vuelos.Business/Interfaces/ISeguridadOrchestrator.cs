using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Portal;

namespace Middleware.Vuelos.Business.Interfaces;

public interface ISeguridadOrchestrator
{
    /// <summary>
    /// Autentica credenciales contra MS Seguridad y devuelve el JWT.
    /// Endpoint público — no requiere token previo.
    /// </summary>
    Task<LoginResponse> LoginAsync(LoginRequest request);
}