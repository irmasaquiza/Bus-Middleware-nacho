using Middleware.Vuelos.DataManagement.Models.Aeropuertos;

namespace Middleware.Vuelos.DataManagement.Interfaces;

/// <summary>
/// Contrato del servicio de datos de Aeropuertos en el Bus.
/// Abstrae las llamadas al AeropuertosClient y mapea DTOs
/// a modelos internos del Bus.
///
/// Este servicio NO valida país ni ciudad — eso lo hace
/// IGeografiaDataService antes de llegar aquí.
/// </summary>
public interface IAeropuertosDataService
{
    /// <summary>
    /// Obtiene un aeropuerto por su id.
    /// </summary>
    Task<AeropuertoDataModel?> GetByIdAsync(int idAeropuerto);

    /// <summary>
    /// Obtiene un aeropuerto por su código IATA.
    /// Útil para validar duplicados antes de crear.
    /// </summary>
    Task<AeropuertoDataModel?> GetByCodigoIataAsync(string codigoIata);

    /// <summary>
    /// Crea un aeropuerto en MS Aeropuertos.
    /// El Bus ya debe haber validado país y ciudad con IGeografiaDataService.
    /// Requiere JWT con rol ADMINISTRADOR o AEROLINEA.
    /// </summary>
    Task<AeropuertoDataModel?> CrearAsync(
        string codigoIata,
        string? codigoIcao,
        string nombre,
        int idPais,
        int? idCiudad,
        string? zonaHoraria,
        decimal? latitud,
        decimal? longitud,
        string jwtToken);

    /// <summary>
    /// Elimina lógicamente un aeropuerto.
    /// Requiere JWT con rol ADMINISTRADOR.
    /// </summary>
    Task<bool> EliminarAsync(int idAeropuerto, string jwtToken);
}