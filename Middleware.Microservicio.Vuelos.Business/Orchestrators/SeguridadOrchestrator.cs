using Microsoft.Extensions.Logging;
using Middleware.Vuelos.Business.DTOs.Seguridad;
using Middleware.Vuelos.Business.Exceptions;
using Middleware.Vuelos.Business.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;

namespace Middleware.Vuelos.Business.Orchestrators;

public class SeguridadOrchestrator : ISeguridadOrchestrator
{
    private readonly ISeguridadDataService _seguridadDataService;
    private readonly ILogger<SeguridadOrchestrator> _logger;

    public SeguridadOrchestrator(
        ISeguridadDataService seguridadDataService,
        ILogger<SeguridadOrchestrator> logger)
    {
        _seguridadDataService = seguridadDataService;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation(
            "[Bus][SeguridadOrchestrator] Login. Username={Username}",
            request.Username);

        var token = await _seguridadDataService.LoginAsync(
            request.Username, request.Password)
            ?? throw new UnauthorizedBusinessException(
                "Usuario o contraseña incorrectos.");

        return new LoginResponse
        {
            Token = token.Token,
            Usuario = token.Usuario,
            Expiracion = token.Expiracion,
            Roles = token.Roles
        };
    }
}