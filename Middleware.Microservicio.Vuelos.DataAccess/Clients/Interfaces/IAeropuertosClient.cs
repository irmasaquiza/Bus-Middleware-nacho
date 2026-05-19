using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.DataAccess.Clients.Interfaces;

/// <summary>
/// Contrato del cliente HTTP que el Bus usa para comunicarse con MS Aeropuertos.
/// Solo REST. URL base dev: https://localhost:44363
///
/// IMPORTANTE: Los endpoints de escritura (POST, PUT, DELETE) requieren JWT.
/// El Bus debe reenviar el token del usuario en el header Authorization.
/// </summary>
public interface IAeropuertosClient
{
    /// <summary>
    /// Obtiene un aeropuerto por su id.
    /// GET /api/v1/aeropuertos/{id_aeropuerto}
    /// Endpoint público — no requiere JWT.
    /// </summary>
    Task<AeropuertoDto?> GetByIdAsync(int idAeropuerto);

    /// <summary>
    /// Obtiene un aeropuerto por su código IATA.
    /// GET /api/v1/aeropuertos?codigoIata={codigo}
    /// Útil para validar duplicados antes de crear.
    /// </summary>
    Task<AeropuertoDto?> GetByCodigoIataAsync(string codigoIata);

    /// <summary>
    /// Crea un aeropuerto en MS Aeropuertos.
    /// POST /api/v1/aeropuertos
    /// Requiere JWT con rol ADMINISTRADOR o AEROLINEA.
    /// El Bus debe pasar el token del usuario en jwtToken.
    /// El Bus ya validó país y ciudad contra MS Geografía antes de llamar aquí.
    /// </summary>
    Task<AeropuertoDto?> CrearAsync(CrearAeropuertoRequestDto request, string jwtToken);

    /// <summary>
    /// Elimina lógicamente un aeropuerto.
    /// DELETE /api/v1/aeropuertos/{id_aeropuerto}
    /// Requiere JWT con rol ADMINISTRADOR.
    /// </summary>
    Task<bool> EliminarAsync(int idAeropuerto, string jwtToken);
}