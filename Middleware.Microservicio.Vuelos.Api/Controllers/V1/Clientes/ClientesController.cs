using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Vuelos.Business.DTOs.Clientes;
using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.Api.Controllers.V1.Clientes
{
    // ── ClientesController ────────────────────────────────────────────────────

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes")]
    [Authorize]
    public class ClientesAdminController : ControllerBase
    {
        private readonly ClientesClient _clientesClient;

        public ClientesAdminController(ClientesClient clientesClient)
        {
            _clientesClient = clientesClient;
        }

        /// <summary>
        /// Lista clientes con filtros y paginación.
        /// GET /api/v1/clientes
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] ClientesFiltroRequest filtro)
        {
            var token = ObtenerToken();
            var result = await _clientesClient.GetClientesPagedAsync(
                filtro.TipoIdentificacion, filtro.NumeroIdentificacion,
                filtro.Nombres, filtro.Correo, filtro.Estado,
                filtro.Page, filtro.PageSize, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene un cliente por id.
        /// GET /api/v1/clientes/{id_cliente}
        /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
        /// </summary>
        [HttpGet("{id_cliente:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
        public async Task<IActionResult> GetById([FromRoute] int id_cliente)
        {
            var token = ObtenerToken();
            var result = await _clientesClient.GetClienteByIdAsync(id_cliente, token);
            return result is null
                ? NotFound(new { success = false, message = "Cliente no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Crea un cliente con su usuario en MS Seguridad.
        /// POST /api/v1/clientes
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// MS Clientes se encarga de crear el usuario en MS Seguridad internamente.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear([FromBody] CrearClienteAdminRequest request)
        {
            var token = ObtenerToken();

            var clienteDto = await _clientesClient.CrearClienteAsync(
                new CrearClienteRequestDto
                {
                    TipoIdentificacion = request.TipoIdentificacion,
                    NumeroIdentificacion = request.NumeroIdentificacion,
                    Nombres = request.Nombres,
                    Apellidos = request.Apellidos,
                    RazonSocial = request.RazonSocial,
                    Correo = request.Correo,
                    Telefono = request.Telefono,
                    Direccion = request.Direccion,
                    IdCiudadResidencia = request.IdCiudadResidencia,
                    IdPaisNacionalidad = request.IdPaisNacionalidad,
                    FechaNacimiento = request.FechaNacimiento,
                    Genero = request.Genero,
                    Username = request.Username,
                    Password = request.Password
                },
                token);

            if (clienteDto is null)
                return BadRequest(new
                {
                    success = false,
                    message = "No se pudo crear el cliente."
                });

            return Created(string.Empty, new
            {
                success = true,
                message = "Cliente creado correctamente.",
                data = new
                {
                    IdCliente = clienteDto.IdCliente,
                    Nombres = clienteDto.Nombres,
                    Correo = clienteDto.Correo
                }
            });
        }

        /// <summary>
        /// Actualiza un cliente.
        /// PUT /api/v1/clientes/{id_cliente}
        /// Roles: ADMINISTRADOR, CLIENTE
        /// </summary>
        [HttpPut("{id_cliente:int}")]
        [Authorize(Roles = "ADMINISTRADOR,CLIENTE")]
        public async Task<IActionResult> Actualizar(
            [FromRoute] int id_cliente,
            [FromBody] ActualizarClienteRequest request)
        {
            var token = ObtenerToken();
            var dto = new ActualizarClienteRequestDto
            {
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                RazonSocial = request.RazonSocial,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                IdCiudadResidencia = request.IdCiudadResidencia,
                IdPaisNacionalidad = request.IdPaisNacionalidad,
                FechaNacimiento = request.FechaNacimiento,
                Genero = request.Genero
            };
            var result = await _clientesClient.ActualizarClienteAsync(
                id_cliente, dto, token);
            return result is null
                ? NotFound(new { success = false, message = "Cliente no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Elimina un cliente.
        /// DELETE /api/v1/clientes/{id_cliente}
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpDelete("{id_cliente:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar([FromRoute] int id_cliente)
        {
            var token = ObtenerToken();
            var result = await _clientesClient.EliminarClienteAsync(id_cliente, token);
            return result
                ? Ok(new { success = true, message = "Cliente eliminado." })
                : NotFound(new { success = false, message = "Cliente no encontrado." });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }

    // ── Portal Cliente — Mi Perfil ────────────────────────────────────────────

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes/portal")]
    [Authorize(Roles = "CLIENTE")]
    public class ClientePortalController : ControllerBase
    {
        private readonly ClientesClient _clientesClient;

        public ClientePortalController(ClientesClient clientesClient)
        {
            _clientesClient = clientesClient;
        }

        [HttpGet("mi-perfil")]
        public async Task<IActionResult> GetMiPerfil()
        {
            var token = ObtenerToken();
            var result = await _clientesClient.GetMiPerfilAsync(token);
            return result is null
                ? NotFound(new { success = false, message = "Perfil no encontrado." })
                : Ok(new { success = true, data = result });
        }

        [HttpPut("mi-perfil")]
        public async Task<IActionResult> ActualizarMiPerfil(
            [FromBody] ActualizarClienteRequest request)
        {
            var token = ObtenerToken();
            var dto = new ActualizarClienteRequestDto
            {
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                RazonSocial = request.RazonSocial,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                IdCiudadResidencia = request.IdCiudadResidencia,
                IdPaisNacionalidad = request.IdPaisNacionalidad,
                FechaNacimiento = request.FechaNacimiento,
                Genero = request.Genero
            };
            var result = await _clientesClient.ActualizarMiPerfilAsync(dto, token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo actualizar el perfil." })
                : Ok(new { success = true, data = result });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }

    // ── PasajerosController ───────────────────────────────────────────────────

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pasajeros")]
    [Authorize]
    public class PasajerosAdminController : ControllerBase
    {
        private readonly ClientesClient _clientesClient;

        public PasajerosAdminController(ClientesClient clientesClient)
        {
            _clientesClient = clientesClient;
        }

        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] PasajerosFiltroRequest filtro)
        {
            var token = ObtenerToken();
            var result = await _clientesClient.GetPasajerosPagedAsync(
                filtro.IdCliente, filtro.NombrePasajero, filtro.NumeroDocumento,
                filtro.Estado, filtro.Page, filtro.PageSize, token);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("{id_pasajero:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
        public async Task<IActionResult> GetById([FromRoute] int id_pasajero)
        {
            var token = ObtenerToken();
            var result = await _clientesClient.GetPasajeroByIdAsync(id_pasajero, token);
            return result is null
                ? NotFound(new { success = false, message = "Pasajero no encontrado." })
                : Ok(new { success = true, data = result });
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
        public async Task<IActionResult> Crear([FromBody] CrearPasajeroRequest request)
        {
            var token = ObtenerToken();
            var result = await _clientesClient.CrearPasajeroAsync(
                new CrearPasajeroRequestDto
                {
                    IdCliente = request.IdCliente,
                    NombrePasajero = request.NombrePasajero,
                    ApellidoPasajero = request.ApellidoPasajero,
                    TipoDocumentoPasajero = request.TipoDocumentoPasajero,
                    NumeroDocumentoPasajero = request.NumeroDocumentoPasajero,
                    FechaNacimientoPasajero = request.FechaNacimientoPasajero,
                    IdPaisNacionalidad = request.IdPaisNacionalidad,
                    EmailContactoPasajero = request.EmailContactoPasajero,
                    TelefonoContactoPasajero = request.TelefonoContactoPasajero,
                    GeneroPasajero = request.GeneroPasajero,
                    RequiereAsistencia = request.RequiereAsistencia,
                    ObservacionesPasajero = request.ObservacionesPasajero
                },
                token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear el pasajero." })
                : Created(string.Empty, new { success = true, data = result });
        }

        [HttpPut("{id_pasajero:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Actualizar(
            [FromRoute] int id_pasajero,
            [FromBody] ActualizarPasajeroRequest request)
        {
            var token = ObtenerToken();
            var result = await _clientesClient.ActualizarPasajeroAsync(
                id_pasajero,
                new ActualizarPasajeroRequestDto
                {
                    NombrePasajero = request.NombrePasajero,
                    ApellidoPasajero = request.ApellidoPasajero,
                    TipoDocumentoPasajero = request.TipoDocumentoPasajero,
                    NumeroDocumentoPasajero = request.NumeroDocumentoPasajero,
                    FechaNacimientoPasajero = request.FechaNacimientoPasajero,
                    IdPaisNacionalidad = request.IdPaisNacionalidad,
                    EmailContactoPasajero = request.EmailContactoPasajero,
                    TelefonoContactoPasajero = request.TelefonoContactoPasajero,
                    GeneroPasajero = request.GeneroPasajero,
                    RequiereAsistencia = request.RequiereAsistencia,
                    ObservacionesPasajero = request.ObservacionesPasajero
                },
                token);
            return result is null
                ? NotFound(new { success = false, message = "Pasajero no encontrado." })
                : Ok(new { success = true, data = result });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }
}