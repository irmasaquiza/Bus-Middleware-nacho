using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Models.Auth;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataManagement.Services;

/// <summary>
/// Implementación del servicio de datos de Seguridad en el Bus.
/// Llama al SeguridadClient (HTTP) y transforma los DTOs de DataAccess
/// en modelos internos del Bus (DataManagement).
/// </summary>
public class SeguridadDataService : ISeguridadDataService
{
    private readonly ISeguridadClient _seguridadClient;
    private readonly ILogger<SeguridadDataService> _logger;

    public SeguridadDataService(
        ISeguridadClient seguridadClient,
        ILogger<SeguridadDataService> logger)
    {
        _seguridadClient = seguridadClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TokenDataModel?> LoginAsync(string username, string password)
    {
        _logger.LogInformation(
            "[Bus][SeguridadDataService] Login. Username={Username}", username);

        var dto = await _seguridadClient.LoginAsync(new LoginRequestDto
        {
            Username = username,
            Password = password
        });

        if (dto is null)
        {
            _logger.LogWarning(
                "[Bus][SeguridadDataService] Login devolvió null. Username={Username}",
                username);
            return null;
        }

        return new TokenDataModel
        {
            Token = dto.Token,
            Usuario = dto.Usuario,
            Expiracion = dto.Expiracion,
            Roles = dto.Roles
        };
    }

    /// <inheritdoc />
    public async Task<UsuarioClienteCreado?> CreateUserForClientAsync(
        int idCliente,
        string username,
        string correo,
        string password,
        string correlationId)
    {
        _logger.LogInformation(
            "[Bus][SeguridadDataService] CreateUserForClient. " +
            "IdCliente={IdCliente} Username={Username} CorrelationId={CorrelationId}",
            idCliente, username, correlationId);

        var dto = await _seguridadClient.CreateUserForClientAsync(
            new CreateUserForClientRequestDto
            {
                IdCliente = idCliente,
                Username = username,
                Correo = correo,
                Password = password,
                CorrelationId = correlationId
            });

        if (dto is null)
        {
            _logger.LogWarning(
                "[Bus][SeguridadDataService] CreateUserForClient devolvió null. " +
                "IdCliente={IdCliente} CorrelationId={CorrelationId}",
                idCliente, correlationId);
            return null;
        }

        return new UsuarioClienteCreado
        {
            IdUsuario = dto.IdUsuario,
            UsuarioGuid = dto.UsuarioGuid,
            IdCliente = dto.IdCliente,
            Username = dto.Username,
            RolAsignado = dto.RolAsignado,
            CorrelationId = dto.CorrelationId
        };
    }
}