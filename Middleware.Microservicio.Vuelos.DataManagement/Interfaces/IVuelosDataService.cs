using Middleware.Vuelos.DataManagement.Models.Vuelos;

namespace Middleware.Vuelos.DataManagement.Interfaces;

/// <summary>
/// Contrato del servicio de datos de Vuelos en el Bus.
/// Abstrae las llamadas al VuelosClient y mapea DTOs
/// a modelos internos del Bus.
///
/// Flujo crítico: reserva de asientos coordinada con MS ReservasF.
/// MS Vuelos es dueño del inventario operativo de asientos.
/// MS ReservasF es dueño de las reservas, boletos y facturas.
/// El Bus coordina ambos sin que uno escriba en la BD del otro.
/// </summary>
public interface IVuelosDataService
{
    /// <summary>
    /// Obtiene un vuelo por su id.
    /// Devuelve null si no existe.
    /// </summary>
    Task<VueloDataModel?> GetVueloByIdAsync(int idVuelo);

    /// <summary>
    /// Valida que un vuelo existe, está activo y es operable.
    /// Estados operables: PROGRAMADO, DEMORADO.
    /// Devuelve el vuelo si es válido, null si no.
    /// Usado antes de crear reservas.
    /// </summary>
    Task<VueloDataModel?> ValidarVueloOperableAsync(int idVuelo);

    /// <summary>
    /// Obtiene todos los asientos de un vuelo.
    /// </summary>
    Task<List<AsientoDataModel>> GetAsientosByVueloAsync(int idVuelo);

    /// <summary>
    /// Obtiene un asiento específico de un vuelo.
    /// </summary>
    Task<AsientoDataModel?> GetAsientoByIdAsync(int idVuelo, int idAsiento);

    /// <summary>
    /// Valida que un asiento existe, está activo y disponible para reservar.
    /// Devuelve el asiento si es válido, null si no.
    /// </summary>
    Task<AsientoDataModel?> ValidarAsientoDisponibleAsync(int idVuelo, int idAsiento);

    /// <summary>
    /// Marca un asiento como NO disponible.
    /// Llamado por el Bus cuando MS ReservasF confirma el pago de una reserva.
    /// Requiere JWT con rol ADMINISTRADOR o AEROLINEA.
    /// </summary>
    Task<bool> BloquearAsientoAsync(int idVuelo, int idAsiento, string jwtToken);

    /// <summary>
    /// Marca un asiento como disponible nuevamente.
    /// Llamado por el Bus cuando MS ReservasF cancela una reserva.
    /// Requiere JWT con rol ADMINISTRADOR o AEROLINEA.
    /// </summary>
    Task<bool> LiberarAsientoAsync(int idVuelo, int idAsiento, string jwtToken);

    /// <summary>
    /// Obtiene las escalas de un vuelo.
    /// </summary>
    Task<List<EscalaDataModel>> GetEscalasByVueloAsync(int idVuelo);
}