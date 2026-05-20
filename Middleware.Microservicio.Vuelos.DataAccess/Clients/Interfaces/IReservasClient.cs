using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.DataAccess.Clients.Interfaces;

/// <summary>
/// Contrato del cliente HTTP que el Bus usa para comunicarse con MS ReservasF.
/// Solo REST. URL base dev: https://localhost:44370
///
/// TODOS los endpoints requieren JWT.
/// Para rol CLIENTE, id_cliente viene del JWT — no del body.
/// Para procesos internos del Bus usar token con rol ADMINISTRADOR o AEROLINEA.
///
/// Flujo crítico del Bus:
///   1. Crear reserva → POST /api/v1/reservas
///   2. Pagar reserva → PATCH /api/v1/reservas/{id}/pagar
///      (coordina con MS Vuelos para bloquear asientos)
/// </summary>
public interface IReservasFClient
{
    // ── RESERVAS ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Obtiene una reserva por su id.
    /// GET /api/v1/reservas/{id_reserva}
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<ReservaDto?> GetReservaByIdAsync(int idReserva, string jwtToken);

    /// <summary>
    /// Crea una reserva con sus detalles (pasajeros/asientos).
    /// POST /api/v1/reservas
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// Nota: Para CLIENTE, id_cliente viene del JWT.
    /// </summary>
    Task<ReservaDto?> CrearReservaAsync(CrearReservaRequestDto request, string jwtToken);

    /// <summary>
    /// Procesa el pago de una reserva.
    /// PATCH /api/v1/reservas/{id_reserva}/pagar
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// Este es el endpoint más crítico para el Bus — coordina
    /// reserva, factura, boletos y disponibilidad de asientos en MS Vuelos.
    /// </summary>
    Task<ReservaDto?> PagarReservaAsync(
    int idReserva,
    PagarReservaRequestDto request,
    string jwtToken);

    /// <summary>
    /// Cancela una reserva.
    /// PATCH /api/v1/reservas/{id_reserva}/estado
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<ReservaDto?> CancelarReservaAsync(int idReserva, string motivo, string jwtToken);

    // ── BOLETOS ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Obtiene un boleto por su id.
    /// GET /api/v1/boletos/{id_boleto}
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<BoletoDto?> GetBoletoByIdAsync(int idBoleto, string jwtToken);

    /// <summary>
    /// Obtiene todos los boletos de una reserva.
    /// GET /api/v1/boletos?idReserva={id_reserva}
    /// Roles: ADMINISTRADOR, AEROLINEA
    /// </summary>
    Task<List<BoletoDto>> GetBoletosByReservaAsync(int idReserva, string jwtToken);

    // ── FACTURAS ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Obtiene una factura por su id.
    /// GET /api/v1/facturas/{id_factura}
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<FacturaDto?> GetFacturaByIdAsync(int idFactura, string jwtToken);

    /// <summary>
    /// Obtiene la factura de una reserva.
    /// GET /api/v1/facturas?idReserva={id_reserva}
    /// Roles: ADMINISTRADOR, AEROLINEA
    /// </summary>
    Task<FacturaDto?> GetFacturaByReservaAsync(int idReserva, string jwtToken);

    // ── EQUIPAJE ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Obtiene el equipaje de un boleto.
    /// GET /api/v1/boletos/{id_boleto}/equipaje
    /// Roles: ADMINISTRADOR, AEROLINEA
    /// </summary>
    Task<List<EquipajeDto>> GetEquipajeByBoletoAsync(int idBoleto, string jwtToken);

    /// <summary>
    /// Agrega equipaje a un boleto existente.
    /// POST /api/v1/boletos/{id_boleto}/equipaje
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<EquipajeDto?> AgregarEquipajeAsync(
        int idBoleto,
        AgregarEquipajeRequestDto request,
        string jwtToken);

    /// <summary>
    /// Obtiene reservas paginadas con filtros.
    /// GET /api/v1/reservas
    /// </summary>
    Task<ReservasFPagedDto<ReservaDto>?> GetReservasPagedAsync(
        int? idCliente, int? idVuelo, string? codigoReserva,
        string? estadoReserva, int page, int pageSize, string jwtToken);

    /// <summary>
    /// Obtiene boletos paginados con filtros.
    /// GET /api/v1/boletos
    /// </summary>
    Task<ReservasFPagedDto<BoletoDto>?> GetBoletosPagedAsync(
        int? idReserva, int? idVuelo, string? codigoBoleto,
        string? estadoBoleto, int page, int pageSize, string jwtToken);

    /// <summary>
    /// Obtiene facturas paginadas con filtros.
    /// GET /api/v1/facturas
    /// </summary>
    Task<ReservasFPagedDto<FacturaDto>?> GetFacturasPagedAsync(
        int? idCliente, int? idReserva, string? numeroFactura,
        string? estadoFactura, int page, int pageSize, string jwtToken);
}