using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Portal;

namespace Middleware.Vuelos.Business.Interfaces;

public interface IReservaOrchestrator
{
    /// <summary>
    /// Crea una reserva coordinando MS Vuelos, MS Clientes y MS ReservasF.
    /// El id_cliente se extrae del JWT automáticamente.
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<ReservaResponse> CrearReservaAsync(
        CrearReservaRequest request,
        int idCliente,
        string jwtToken);

    /// <summary>
    /// Procesa el pago de una reserva.
    /// Coordina MS ReservasF (pago) y MS Vuelos (bloqueo de asientos).
    /// Si algo falla, ejecuta compensaciones automáticas.
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<ReservaResponse> PagarReservaAsync(
        int idReserva,
        PagarReservaRequest request,
        string jwtToken);

    /// <summary>
    /// Cancela una reserva.
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<ReservaResponse> CancelarReservaAsync(
        int idReserva,
        CancelarReservaRequest request,
        string jwtToken);

    /// <summary>
    /// Obtiene una reserva por su id.
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<ReservaResponse> GetReservaByIdAsync(
        int idReserva,
        string jwtToken);

    /// <summary>
    /// Obtiene los boletos de una reserva.
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<List<BoletoResponse>> GetBoletosByReservaAsync(
        int idReserva,
        string jwtToken);
}