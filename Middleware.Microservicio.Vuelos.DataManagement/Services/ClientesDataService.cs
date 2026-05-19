using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataAccess.Models;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Models.Clientes;
using Microsoft.Extensions.Logging;

namespace Middleware.Vuelos.DataManagement.Services;

/// <summary>
/// Implementación del servicio de datos de Clientes en el Bus.
/// Llama al ClientesClient (HTTP) y transforma DTOs en modelos internos.
/// </summary>
public class ClientesDataService : IClientesDataService
{
    private readonly IClientesClient _clientesClient;
    private readonly ILogger<ClientesDataService> _logger;

    public ClientesDataService(
        IClientesClient clientesClient,
        ILogger<ClientesDataService> logger)
    {
        _clientesClient = clientesClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ClienteDataModel?> GetClienteByIdAsync(
        int idCliente, string jwtToken)
    {
        var dto = await _clientesClient.GetClienteByIdAsync(idCliente, jwtToken);
        return dto is null ? null : MapClienteToModel(dto);
    }

    /// <inheritdoc />
    public async Task<ClienteDataModel?> CrearClienteAsync(
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
        string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][ClientesDataService] CrearCliente. " +
            "NumeroIdentificacion={NumeroIdentificacion} Correo={Correo}",
            numeroIdentificacion, correo);

        var dto = await _clientesClient.CrearClienteAsync(
            new CrearClienteRequestDto
            {
                TipoIdentificacion = tipoIdentificacion,
                NumeroIdentificacion = numeroIdentificacion,
                Nombres = nombres,
                Apellidos = apellidos,
                RazonSocial = razonSocial,
                Correo = correo,
                Telefono = telefono,
                Direccion = direccion,
                IdCiudadResidencia = idCiudadResidencia,
                IdPaisNacionalidad = idPaisNacionalidad,
                FechaNacimiento = fechaNacimiento,
                Genero = genero
            },
            jwtToken);

        return dto is null ? null : MapClienteToModel(dto);
    }

    /// <inheritdoc />
    public async Task<ClienteDataModel?> ValidarClienteActivoAsync(
        int idCliente, string jwtToken)
    {
        var cliente = await GetClienteByIdAsync(idCliente, jwtToken);

        if (cliente is null)
        {
            _logger.LogWarning(
                "[Bus][ClientesDataService] Cliente no encontrado. IdCliente={IdCliente}",
                idCliente);
            return null;
        }

        if (!cliente.EsActivo)
        {
            _logger.LogWarning(
                "[Bus][ClientesDataService] Cliente inactivo o eliminado. " +
                "IdCliente={IdCliente} Estado={Estado} EsEliminado={EsEliminado}",
                idCliente, cliente.Estado, cliente.EsEliminado);
            return null;
        }

        return cliente;
    }

    /// <inheritdoc />
    public async Task<PasajeroDataModel?> GetPasajeroByIdAsync(
        int idPasajero, string jwtToken)
    {
        var dto = await _clientesClient.GetPasajeroByIdAsync(idPasajero, jwtToken);
        return dto is null ? null : MapPasajeroToModel(dto);
    }

    /// <inheritdoc />
    public async Task<PasajeroDataModel?> ValidarPasajeroActivoAsync(
        int idPasajero, string jwtToken)
    {
        var pasajero = await GetPasajeroByIdAsync(idPasajero, jwtToken);

        if (pasajero is null)
        {
            _logger.LogWarning(
                "[Bus][ClientesDataService] Pasajero no encontrado. " +
                "IdPasajero={IdPasajero}", idPasajero);
            return null;
        }

        if (!pasajero.EsActivo)
        {
            _logger.LogWarning(
                "[Bus][ClientesDataService] Pasajero inactivo o eliminado. " +
                "IdPasajero={IdPasajero} Estado={Estado}",
                idPasajero, pasajero.Estado);
            return null;
        }

        return pasajero;
    }

    /// <inheritdoc />
    public async Task<PasajeroDataModel?> CrearPasajeroAsync(
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
        string jwtToken)
    {
        _logger.LogInformation(
            "[Bus][ClientesDataService] CrearPasajero. " +
            "IdCliente={IdCliente} NumeroDocumento={NumeroDocumento}",
            idCliente, numeroDocumento);

        var dto = await _clientesClient.CrearPasajeroAsync(
            new CrearPasajeroRequestDto
            {
                IdCliente = idCliente,
                NombrePasajero = nombrePasajero,
                ApellidoPasajero = apellidoPasajero,
                TipoDocumentoPasajero = tipoDocumento,
                NumeroDocumentoPasajero = numeroDocumento,
                FechaNacimientoPasajero = fechaNacimiento,
                IdPaisNacionalidad = idPaisNacionalidad,
                EmailContactoPasajero = email,
                TelefonoContactoPasajero = telefono,
                GeneroPasajero = genero,
                RequiereAsistencia = requiereAsistencia,
                ObservacionesPasajero = observaciones
            },
            jwtToken);

        return dto is null ? null : MapPasajeroToModel(dto);
    }

    // ── Mappers privados ─────────────────────────────────────────────────────────

    private static ClienteDataModel MapClienteToModel(ClienteDto dto) =>
        new()
        {
            IdCliente = dto.IdCliente,
            ClienteGuid = dto.ClienteGuid,
            TipoIdentificacion = dto.TipoIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            RazonSocial = dto.RazonSocial,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            IdCiudadResidencia = dto.IdCiudadResidencia,
            IdPaisNacionalidad = dto.IdPaisNacionalidad,
            FechaNacimiento = dto.FechaNacimiento,
            Genero = dto.Genero,
            Estado = dto.Estado,
            EsEliminado = dto.EsEliminado
        };

    private static PasajeroDataModel MapPasajeroToModel(PasajeroDto dto) =>
        new()
        {
            IdPasajero = dto.IdPasajero,
            IdCliente = dto.IdCliente,
            NombrePasajero = dto.NombrePasajero,
            ApellidoPasajero = dto.ApellidoPasajero,
            TipoDocumentoPasajero = dto.TipoDocumentoPasajero,
            NumeroDocumentoPasajero = dto.NumeroDocumentoPasajero,
            FechaNacimientoPasajero = dto.FechaNacimientoPasajero,
            IdPaisNacionalidad = dto.IdPaisNacionalidad,
            EmailContactoPasajero = dto.EmailContactoPasajero,
            TelefonoContactoPasajero = dto.TelefonoContactoPasajero,
            GeneroPasajero = dto.GeneroPasajero,
            RequiereAsistencia = dto.RequiereAsistencia,
            ObservacionesPasajero = dto.ObservacionesPasajero,
            Estado = dto.Estado,
            EsEliminado = dto.EsEliminado
        };
}