using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Portal;

namespace Middleware.Vuelos.Business.Interfaces;

public interface IFacturacionOrchestrator
{
    /// <summary>
    /// Obtiene la factura de una reserva.
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<FacturaResponse> GetFacturaByReservaAsync(
        int idReserva,
        string jwtToken);

    /// <summary>
    /// Obtiene una factura por su id.
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<FacturaResponse> GetFacturaByIdAsync(
        int idFactura,
        string jwtToken);
}
