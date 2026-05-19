using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.DataAccess.Clients.Interfaces;

/// <summary>
/// Contrato del cliente HTTP que el Bus usa para comunicarse con MS Vuelos.
/// Solo REST. URL base dev: https://localhost:44385
///
/// Endpoints de lectura son públicos (no requieren JWT).
/// Endpoints de escritura requieren JWT con rol ADMINISTRADOR o AEROLINEA.
/// El Bus reenvía el token del usuario en jwtToken cuando aplica.
/// </summary>
public interface IVuelosClient
{
    /// <summary>
    /// Obtiene un vuelo por su id.
    /// GET /api/v1/vuelos/{id_vuelo}
    /// Público — no requiere JWT.
    /// </summary>
    Task<VueloDto?> GetVueloByIdAsync(int idVuelo);

    /// <summary>
    /// Obtiene todos los asientos de un vuelo.
    /// GET /api/v1/vuelos/{id_vuelo}/asientos
    /// Público — no requiere JWT.
    /// </summary>
    Task<List<AsientoDto>> GetAsientosByVueloAsync(int idVuelo);

    /// <summary>
    /// Obtiene un asiento específico de un vuelo.
    /// GET /api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
    /// Público — no requiere JWT.
    /// </summary>
    Task<AsientoDto?> GetAsientoByIdAsync(int idVuelo, int idAsiento);

    /// <summary>
    /// Obtiene las escalas de un vuelo.
    /// GET /api/v1/vuelos/{id_vuelo}/escalas
    /// Público — no requiere JWT.
    /// </summary>
    Task<List<EscalaDto>> GetEscalasByVueloAsync(int idVuelo);

    /// <summary>
    /// Actualiza la disponibilidad de un asiento.
    /// PATCH /api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
    /// Roles: ADMINISTRADOR, AEROLINEA
    ///
    /// Usado por el Bus al coordinar reservas con MS ReservasF.
    /// Cuando ReservasF paga una reserva, el Bus llama aquí
    /// para marcar el asiento como no disponible.
    /// </summary>
    Task<AsientoDto?> ActualizarDisponibilidadAsientoAsync(
        int idVuelo,
        int idAsiento,
        bool disponible,
        string jwtToken);
}