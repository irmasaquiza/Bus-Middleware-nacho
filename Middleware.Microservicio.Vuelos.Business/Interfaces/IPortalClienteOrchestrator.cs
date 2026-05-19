using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Portal;

namespace Middleware.Vuelos.Business.Interfaces;

public interface IPortalClienteOrchestrator
{
    /// <summary>
    /// Lista todas las reservas del cliente autenticado.
    /// GET /api/v1/cliente/reservas (migrado)
    /// id_cliente viene del JWT.
    /// </summary>
    Task<List<ReservaPortalResponse>> GetMisReservasAsync(
        int idCliente,
        string jwtToken);

    /// <summary>
    /// Obtiene el detalle de una reserva específica del cliente.
    /// GET /api/v1/cliente/reservas/{id}/detalle (migrado)
    /// </summary>
    Task<ReservaPortalResponse> GetDetalleReservaAsync(
        int idReserva,
        int idCliente,
        string jwtToken);

    /// <summary>
    /// Obtiene una reserva por su código.
    /// GET /api/v1/cliente/reservas/by-codigo/{codigo} (migrado)
    /// </summary>
    Task<ReservaPortalResponse> GetReservaPorCodigoAsync(
        string codigoReserva,
        int idCliente,
        string jwtToken);

    /// <summary>
    /// Obtiene la factura de una reserva del cliente.
    /// GET /api/v1/cliente/reservas/{id}/factura (migrado)
    /// </summary>
    Task<FacturaPortalResponse> GetFacturaDeReservaAsync(
        int idReserva,
        int idCliente,
        string jwtToken);

    /// <summary>
    /// Lista todos los boletos del cliente autenticado.
    /// GET /api/v1/cliente/boletos (migrado)
    /// </summary>
    Task<List<BoletoPortalResponse>> GetMisBoletosAsync(
        int idCliente,
        string jwtToken);

    /// <summary>
    /// Obtiene el boleto de una reserva específica.
    /// GET /api/v1/cliente/reservas/{id}/boleto (migrado)
    /// </summary>
    Task<BoletoPortalResponse> GetBoletoDeReservaAsync(
        int idReserva,
        int idCliente,
        string jwtToken);

    /// <summary>
    /// Lista todas las facturas del cliente autenticado.
    /// GET /api/v1/cliente/facturas (migrado)
    /// </summary>
    Task<List<FacturaPortalResponse>> GetMisFacturasAsync(
        int idCliente,
        string jwtToken);
}