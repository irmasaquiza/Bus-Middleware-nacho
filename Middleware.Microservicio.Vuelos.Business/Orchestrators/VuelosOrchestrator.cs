using Microsoft.Extensions.Logging;
using Middleware.Vuelos.Business.Exceptions;
using Middleware.Vuelos.Business.Mappers;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.Business.DTOs.Facturacion;
using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.Business.Exceptions;
using Middleware.Vuelos.Business.Interfaces;
using Middleware.Vuelos.Business.Mappers;
using Middleware.Vuelos.DataManagement.Interfaces;

namespace Middleware.Vuelos.Business.Orchestrators;

public class VuelosOrchestrator : IVuelosOrchestrator
{
    private readonly IVuelosDataService _vuelosDataService;
    private readonly ILogger<VuelosOrchestrator> _logger;

    public VuelosOrchestrator(
        IVuelosDataService vuelosDataService,
        ILogger<VuelosOrchestrator> logger)
    {
        _vuelosDataService = vuelosDataService;
        _logger = logger;
    }

    public async Task<List<VueloResponse>> BuscarVuelosAsync(BuscarVuelosRequest request)
    {
        _logger.LogInformation(
            "[Bus][VuelosOrchestrator] BuscarVuelos. " +
            "Origen={Origen} Destino={Destino} Fecha={Fecha}",
            request.IdAeropuertoOrigen,
            request.IdAeropuertoDestino,
            request.FechaSalida);

        // Por ahora devuelve todos los vuelos disponibles
        // TODO: implementar filtros cuando MS Vuelos exponga endpoint de búsqueda
        throw new BusinessException(
            "La búsqueda de vuelos aún no está implementada en MS Vuelos.");
    }

    public async Task<VueloResponse> GetVueloByIdAsync(int idVuelo)
    {
        var vuelo = await _vuelosDataService.GetVueloByIdAsync(idVuelo)
            ?? throw new NotFoundException($"El vuelo {idVuelo} no existe.");

        var escalas = await _vuelosDataService.GetEscalasByVueloAsync(idVuelo);

        return VuelosBusinessMapper.ToVueloResponse(vuelo, escalas);
    }

    public async Task<List<AsientoResponse>> GetAsientosAsync(int idVuelo)
    {
        var vuelo = await _vuelosDataService.GetVueloByIdAsync(idVuelo);
        if (vuelo is null)
            throw new NotFoundException($"El vuelo {idVuelo} no existe.");

        var asientos = await _vuelosDataService.GetAsientosByVueloAsync(idVuelo);
        return asientos.Select(VuelosBusinessMapper.ToAsientoResponse).ToList();
    }

    public async Task<List<EscalaResponse>> GetEscalasAsync(int idVuelo)
    {
        var vuelo = await _vuelosDataService.GetVueloByIdAsync(idVuelo);
        if (vuelo is null)
            throw new NotFoundException($"El vuelo {idVuelo} no existe.");

        var escalas = await _vuelosDataService.GetEscalasByVueloAsync(idVuelo);
        return escalas.Select(VuelosBusinessMapper.ToEscalaResponse).ToList();
    }
}
