using Middleware.Vuelos.DataManagement.Models.Clientes;

namespace Middleware.Vuelos.DataManagement.Interfaces;

/// <summary>
/// Contrato del servicio de datos de Clientes en el Bus.
/// Abstrae las llamadas al ClientesClient y mapea DTOs
/// a modelos internos del Bus.
/// </summary>
public interface IClientesDataService
{
    /// <summary>
    /// Obtiene un cliente por su id.
    /// Devuelve null si no existe.
    /// </summary>
    Task<ClienteDataModel?> GetClienteByIdAsync(int idCliente, string jwtToken);

    /// <summary>
    /// Crea un cliente en MS Clientes desde flujo administrativo.
    /// Roles requeridos en jwtToken: ADMINISTRADOR, AEROLINEA.
    /// </summary>
    Task<ClienteDataModel?> CrearClienteAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        string nombres,
        string? apellidos,
        string? razonSocial,
        string correo,
        string telefono,
        string direccion,
        int idCiudadResidencia,
        int idPaisNacionalidad,
        DateTime? fechaNacimiento,
        string? genero,
        string jwtToken);

    /// <summary>
    /// Valida que un cliente existe y está activo.
    /// Devuelve el cliente si es válido, null si no.
    /// Usado por MS ReservasF antes de crear reservas.
    /// </summary>
    Task<ClienteDataModel?> ValidarClienteActivoAsync(int idCliente, string jwtToken);

    /// <summary>
    /// Obtiene un pasajero por su id.
    /// </summary>
    Task<PasajeroDataModel?> GetPasajeroByIdAsync(int idPasajero, string jwtToken);

    /// <summary>
    /// Valida que un pasajero existe y está activo.
    /// </summary>
    Task<PasajeroDataModel?> ValidarPasajeroActivoAsync(int idPasajero, string jwtToken);

    /// <summary>
    /// Crea un pasajero en MS Clientes.
    /// Roles requeridos: ADMINISTRADOR, AEROLINEA, CLIENTE.
    /// </summary>
    Task<PasajeroDataModel?> CrearPasajeroAsync(
        int? idCliente,
        string nombrePasajero,
        string apellidoPasajero,
        string tipoDocumento,
        string numeroDocumento,
        DateTime? fechaNacimiento,
        int? idPaisNacionalidad,
        string? email,
        string? telefono,
        string? genero,
        bool requiereAsistencia,
        string? observaciones,
        string jwtToken);
}