using Middleware.Vuelos.DataManagement.Models.Reservas;

namespace Middleware.Vuelos.DataManagement.Interfaces;

/// <summary>
/// Contrato del servicio de datos de ReservasF en el Bus.
///
/// Flujo crítico coordinado por el Bus:
///   1. Validar vuelo operable → IVuelosDataService
///   2. Validar cliente activo → IClientesDataService
///   3. Validar asientos disponibles → IVuelosDataService
///   4. Crear reserva → IReservasDataService.CrearReservaAsync
///   5. Pagar reserva → IReservasDataService.PagarReservaAsync
///      (ReservasF crea factura y boletos internamente)
///   6. Bloquear asientos → IVuelosDataService.BloquearAsientoAsync
///
/// Compensación si falla paso 6:
///   → IReservasDataService.CancelarReservaAsync
/// </summary>
public interface IReservasDataService
{
    // ── RESERVAS ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Obtiene una reserva por su id con sus detalles.
    /// </summary>
    Task<ReservaDataModel?> GetReservaByIdAsync(int idReserva, string jwtToken);

    /// <summary>
    /// Crea una reserva en MS ReservasF.
    /// Prerrequisito: el Bus ya validó vuelo, cliente y asientos.
    /// </summary>
    Task<ReservaDataModel?> CrearReservaAsync(
        int idCliente,
        int idVuelo,
        DateTime fechaInicio,
        DateTime fechaFin,
        string? contactoEmail,
        string? contactoTelefono,
        string? observaciones,
        List<(int idPasajero, int idAsiento)> detalles,
        string jwtToken);

    /// <summary>
    /// Procesa el pago de una reserva.
    /// MS ReservasF crea la factura y los boletos internamente.
    /// El Bus debe bloquear los asientos en MS Vuelos después de este paso.
    /// </summary>
    Task<ReservaDataModel?> PagarReservaAsync(int idReserva, string jwtToken);

    /// <summary>
    /// Cancela una reserva.
    /// Usado como compensación si algo falla después de crear la reserva.
    /// </summary>
    Task<ReservaDataModel?> CancelarReservaAsync(
        int idReserva, string motivo, string jwtToken);

    /// <summary>
    /// Valida que una reserva existe y está en estado pagable.
    /// Estados pagables: PEN, CON.
    /// </summary>
    Task<ReservaDataModel?> ValidarReservaPagableAsync(int idReserva, string jwtToken);

    // ── BOLETOS ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Obtiene un boleto por su id.
    /// </summary>
    Task<BoletoDataModel?> GetBoletoByIdAsync(int idBoleto, string jwtToken);

    /// <summary>
    /// Obtiene todos los boletos de una reserva.
    /// </summary>
    Task<List<BoletoDataModel>> GetBoletosByReservaAsync(int idReserva, string jwtToken);

    // ── FACTURAS ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Obtiene la factura de una reserva.
    /// Una reserva tiene exactamente una factura.
    /// </summary>
    Task<FacturaDataModel?> GetFacturaByReservaAsync(int idReserva, string jwtToken);

    // ── EQUIPAJE ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Obtiene el equipaje de un boleto.
    /// </summary>
    Task<List<EquipajeDataModel>> GetEquipajeByBoletoAsync(
        int idBoleto, string jwtToken);

    /// <summary>
    /// Agrega equipaje a un boleto existente.
    /// Precio fijo: $45 por maleta de bodega.
    /// </summary>
    Task<EquipajeDataModel?> AgregarEquipajeAsync(
        int idBoleto,
        string tipo,
        decimal pesoKg,
        string? descripcion,
        string? dimensionesCm,
        string jwtToken);
}