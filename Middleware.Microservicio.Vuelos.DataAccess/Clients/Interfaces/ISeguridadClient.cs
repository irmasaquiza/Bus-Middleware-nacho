using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.DataAccess.Clients.Interfaces;

/// <summary>
/// Contrato del cliente HTTP que el Bus usa para comunicarse con MS Seguridad.
/// Encapsula todas las llamadas REST hacia http://localhost:5062 (dev).
/// Solo REST, sin gRPC (MS Seguridad no tiene gRPC habilitado).
/// </summary>
public interface ISeguridadClient
{
    /// <summary>
    /// Autentica credenciales contra MS Seguridad y obtiene un JWT.
    /// POST /api/v1/auth/login
    /// Endpoint público, no requiere token previo.
    /// </summary>
    /// <param name="request">Username y password.</param>
    /// <returns>Token JWT, usuario, expiración y roles.</returns>
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    /// <summary>
    /// Crea un usuario de aplicación en MS Seguridad con el IdCliente real vinculado.
    /// POST /api/v1/internal/seguridad/users/create-for-client
    ///
    /// Este endpoint fue creado específicamente para el Bus.
    /// Resuelve el problema de register-cliente donde IdCliente = 0.
    /// Comportamiento idempotente: si el usuario ya existe con el mismo IdCliente,
    /// devuelve los datos existentes sin duplicar.
    /// </summary>
    /// <param name="request">IdCliente real, username, correo, password y correlationId.</param>
    /// <returns>Datos del usuario creado o existente.</returns>
    Task<CreateUserForClientResponseDto?> CreateUserForClientAsync(
        CreateUserForClientRequestDto request);
}