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

public class FacturacionOrchestrator : IFacturacionOrchestrator
{
    private readonly IReservasDataService _reservasDataService;
    private readonly ILogger<FacturacionOrchestrator> _logger;

    public FacturacionOrchestrator(
        IReservasDataService reservasDataService,
        ILogger<FacturacionOrchestrator> logger)
    {
        _reservasDataService = reservasDataService;
        _logger = logger;
    }

    public async Task<FacturaResponse> GetFacturaByReservaAsync(
        int idReserva, string jwtToken)
    {
        var factura = await _reservasDataService.GetFacturaByReservaAsync(
            idReserva, jwtToken)
            ?? throw new NotFoundException(
                $"No se encontró factura para la reserva {idReserva}.");

        return ReservasBusinessMapper.ToFacturaResponse(factura);
    }

    public async Task<FacturaResponse> GetFacturaByIdAsync(
        int idFactura, string jwtToken)
    {
        // MS ReservasF no expone GetByIdFactura directo en el DataService
        // Se busca usando el endpoint de facturas
        throw new BusinessException(
            "Consulta de factura por id no implementada aún.");
    }
}
