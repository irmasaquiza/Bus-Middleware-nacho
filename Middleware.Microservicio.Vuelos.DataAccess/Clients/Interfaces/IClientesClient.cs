using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.DataAccess.Clients.Interfaces;

/// <summary>
/// Contrato del cliente HTTP que el Bus usa para comunicarse con MS Clientes.
/// Solo REST. URL base dev: https://localhost:44391
///
/// Endpoints protegidos requieren JWT con rol correspondiente.
/// El Bus debe reenviar el token del usuario en jwtToken.
/// </summary>
public interface IClientesClient
{
    /// <summary>
    /// Obtiene un cliente por su id.
    /// GET /api/v1/clientes/{id_cliente}
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<ClienteDto?> GetClienteByIdAsync(int idCliente, string jwtToken);

    /// <summary>
    /// Crea un cliente desde flujo administrativo.
    /// POST /api/v1/clientes
    /// Roles: ADMINISTRADOR, AEROLINEA
    /// </summary>
    Task<ClienteDto?> CrearClienteAsync(CrearClienteRequestDto request, string jwtToken);

    /// <summary>
    /// Obtiene un pasajero por su id.
    /// GET /api/v1/pasajeros/{id_pasajero}
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// </summary>
    Task<PasajeroDto?> GetPasajeroByIdAsync(int idPasajero, string jwtToken);

    /// <summary>
    /// Crea un pasajero.
    /// POST /api/v1/pasajeros
    /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
    /// Nota: Si el rol es CLIENTE, el MS fuerza IdCliente desde el claim id_cliente.
    /// </summary>
    Task<PasajeroDto?> CrearPasajeroAsync(CrearPasajeroRequestDto request, string jwtToken);
}