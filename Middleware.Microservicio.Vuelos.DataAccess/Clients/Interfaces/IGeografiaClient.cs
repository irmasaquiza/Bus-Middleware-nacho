using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.DataAccess.Clients.Interfaces;

/// <summary>
/// Contrato del cliente HTTP que el Bus usa para comunicarse con MS Geografía.
/// Solo REST — MS Geografía no tiene gRPC habilitado.
/// URL base dev: https://localhost:44395
/// Named HttpClient: "GeografiaClient"
/// </summary>
public interface IGeografiaClient
{
    /// <summary>
    /// Obtiene un país por su id interno.
    /// GET /api/v1/paises/{id_pais}
    /// Endpoint público — no requiere JWT.
    /// </summary>
    Task<PaisDto?> GetPaisByIdAsync(int idPais);

    /// <summary>
    /// Obtiene un país por su código ISO2.
    /// GET /api/v1/paises?codigoIso2={codigo}
    /// Útil para validaciones cruzadas desde Aeropuertos o Clientes.
    /// </summary>
    Task<PaisDto?> GetPaisByCodigoIso2Async(string codigoIso2);

    /// <summary>
    /// Obtiene una ciudad por su id interno.
    /// GET /api/v1/ciudades/{id_ciudad}
    /// Endpoint público — no requiere JWT.
    /// </summary>
    Task<CiudadDto?> GetCiudadByIdAsync(int idCiudad);

    /// <summary>
    /// Valida que una ciudad pertenece a un país específico.
    /// Consulta la ciudad y verifica que su IdPais coincida.
    /// Usado por MS Aeropuertos antes de crear/editar aeropuertos.
    /// </summary>
    Task<bool> ValidarCiudadPerteneceAPaisAsync(int idCiudad, int idPais);
}