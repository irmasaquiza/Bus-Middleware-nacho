using Middleware.Vuelos.DataManagement.Models.Auth;

namespace Middleware.Vuelos.DataManagement.Interfaces;

/// <summary>
/// Contrato del servicio de datos de Seguridad en el Bus.
/// Abstrae las llamadas al SeguridadClient y mapea los resultados
/// a modelos internos del Bus.
///
/// Esta interfaz es consumida por la capa Business (Orchestrators).
/// </summary>
public interface ISeguridadDataService
{
    /// <summary>
    /// Autentica credenciales contra MS Seguridad.
    /// Internamente llama a POST /api/v1/auth/login.
    /// </summary>
    /// <param name="username">Nombre de usuario.</param>
    /// <param name="password">Contraseña en texto plano.</param>
    /// <returns>Token y datos de sesión, o null si las credenciales son inválidas.</returns>
    Task<TokenDataModel?> LoginAsync(string username, string password);

    /// <summary>
    /// Solicita a MS Seguridad crear un usuario de aplicación vinculado
    /// al cliente real ya existente en MS Clientes.
    ///
    /// Internamente llama a POST /api/v1/internal/seguridad/users/create-for-client.
    /// Es idempotente: si el usuario ya existe con el mismo IdCliente, devuelve los datos.
    /// </summary>
    /// <param name="idCliente">Id del cliente en MS Clientes.</param>
    /// <param name="username">Username deseado para la cuenta.</param>
    /// <param name="correo">Correo electrónico único.</param>
    /// <param name="password">Contraseña en texto plano. Seguridad la hashea.</param>
    /// <param name="correlationId">Id de correlación de la saga activa.</param>
    /// <returns>Datos del usuario creado o existente.</returns>
    Task<UsuarioClienteCreado?> CreateUserForClientAsync(
        int idCliente,
        string username,
        string correo,
        string password,
        string correlationId);
}