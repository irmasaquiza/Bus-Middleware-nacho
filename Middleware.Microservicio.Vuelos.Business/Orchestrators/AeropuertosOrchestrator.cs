using Microsoft.Extensions.Logging;
using Middleware.Vuelos.Business.Exceptions;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.Exceptions;
using Middleware.Vuelos.Business.Interfaces;
using Middleware.Vuelos.Business.Mappers;
using Middleware.Vuelos.DataManagement.Interfaces;

namespace Middleware.Vuelos.Business.Orchestrators;

public class AeropuertosOrchestrator : IAeropuertosOrchestrator
{
    private readonly IAeropuertosDataService _aeropuertosDataService;
    private readonly ILogger<AeropuertosOrchestrator> _logger;

    public AeropuertosOrchestrator(
        IAeropuertosDataService aeropuertosDataService,
        ILogger<AeropuertosOrchestrator> logger)
    {
        _aeropuertosDataService = aeropuertosDataService;
        _logger = logger;
    }

    public async Task<AeropuertoResponse> GetByIdAsync(int idAeropuerto)
    {
        var aeropuerto = await _aeropuertosDataService.GetByIdAsync(idAeropuerto)
            ?? throw new NotFoundException(
                $"El aeropuerto {idAeropuerto} no existe.");

        return new AeropuertoResponse
        {
            IdAeropuerto = aeropuerto.IdAeropuerto,
            CodigoIata = aeropuerto.CodigoIata,
            CodigoIcao = aeropuerto.CodigoIcao,
            Nombre = aeropuerto.Nombre,
            IdCiudad = aeropuerto.IdCiudad,
            IdPais = aeropuerto.IdPais,
            ZonaHoraria = aeropuerto.ZonaHoraria,
            Latitud = aeropuerto.Latitud,
            Longitud = aeropuerto.Longitud,
            Estado = aeropuerto.Estado
        };
    }

    public async Task<AeropuertoResponse> GetByCodigoIataAsync(string codigoIata)
    {
        var aeropuerto = await _aeropuertosDataService.GetByCodigoIataAsync(codigoIata)
            ?? throw new NotFoundException(
                $"No se encontró aeropuerto con código IATA '{codigoIata}'.");

        return new AeropuertoResponse
        {
            IdAeropuerto = aeropuerto.IdAeropuerto,
            CodigoIata = aeropuerto.CodigoIata,
            CodigoIcao = aeropuerto.CodigoIcao,
            Nombre = aeropuerto.Nombre,
            IdCiudad = aeropuerto.IdCiudad,
            IdPais = aeropuerto.IdPais,
            ZonaHoraria = aeropuerto.ZonaHoraria,
            Latitud = aeropuerto.Latitud,
            Longitud = aeropuerto.Longitud,
            Estado = aeropuerto.Estado
        };
    }
}
