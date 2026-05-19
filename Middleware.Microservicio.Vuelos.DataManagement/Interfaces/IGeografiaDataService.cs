using Middleware.Vuelos.DataManagement.Models.Geografia;

namespace Middleware.Vuelos.DataManagement.Interfaces;

/// <summary>
/// Contrato del servicio de datos de Geografía en el Bus.
/// Abstrae las llamadas al GeografiaClient y mapea los DTOs
/// a modelos internos del Bus.
///
/// Consumido por la capa Business (Orchestrators) del Bus.
/// </summary>
public interface IGeografiaDataService
{
    /// <summary>
    /// Obtiene un país por su id.
    /// Devuelve null si no existe o no está accesible.
    /// </summary>
    Task<PaisDataModel?> GetPaisByIdAsync(int idPais);

    /// <summary>
    /// Obtiene un país por su código ISO2.
    /// Útil para validaciones cruzadas.
    /// </summary>
    Task<PaisDataModel?> GetPaisByCodigoIso2Async(string codigoIso2);

    /// <summary>
    /// Obtiene una ciudad por su id.
    /// Devuelve null si no existe o no está accesible.
    /// </summary>
    Task<CiudadDataModel?> GetCiudadByIdAsync(int idCiudad);

    /// <summary>
    /// Valida que un país existe y está activo.
    /// Devuelve el país si es válido, null si no lo es.
    /// </summary>
    Task<PaisDataModel?> ValidarPaisActivoAsync(int idPais);

    /// <summary>
    /// Valida que una ciudad existe, está activa
    /// y pertenece al país indicado.
    /// Devuelve la ciudad si es válida, null si no lo es.
    /// Usado por MS Aeropuertos antes de crear/editar aeropuertos.
    /// </summary>
    Task<CiudadDataModel?> ValidarCiudadActivaEnPaisAsync(int idCiudad, int idPais);
}