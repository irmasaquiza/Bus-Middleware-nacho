using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Vuelos.Business.DTOs.Geografia;
using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.Api.Controllers.V1.Geografia
{
    // ── PaisesController ──────────────────────────────────────────────────────

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/paises")]
    public class PaisesController : ControllerBase
    {
        private readonly GeografiaClient _geografiaClient;

        public PaisesController(GeografiaClient geografiaClient)
        {
            _geografiaClient = geografiaClient;
        }

        /// <summary>
        /// Lista países con filtros y paginación.
        /// GET /api/v1/paises
        /// Público.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaged(
            [FromQuery] PaisesFiltroRequest filtro)
        {
            var result = await _geografiaClient.GetPaisesAsync(
                filtro.Nombre, filtro.CodigoIso2, filtro.Continente,
                filtro.Estado, filtro.PaginaActual, filtro.TamanoPagina);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene un país por id.
        /// GET /api/v1/paises/{id_pais}
        /// Público.
        /// </summary>
        [HttpGet("{id_pais:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id_pais)
        {
            var result = await _geografiaClient.GetPaisByIdAsync(id_pais);
            return result is null
                ? NotFound(new { success = false, message = "País no encontrado." })
                : Ok(new
                {
                    success = true,
                    data = new PaisResponse
                    {
                        IdPais = result.IdPais,
                        CodigoIso2 = result.CodigoIso2,
                        CodigoIso3 = result.CodigoIso3,
                        Nombre = result.Nombre,
                        Continente = result.Continente,
                        Estado = result.Estado
                    }
                });
        }

        /// <summary>
        /// Crea un nuevo país.
        /// POST /api/v1/paises
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Crear([FromBody] CrearPaisRequest request)
        {
            var dto = new CrearPaisRequestDto
            {
                CodigoIso2 = request.CodigoIso2,
                CodigoIso3 = request.CodigoIso3,
                Nombre = request.Nombre,
                Continente = request.Continente
            };
            var result = await _geografiaClient.CrearPaisAsync(dto);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear el país." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Actualiza un país.
        /// PUT /api/v1/paises/{id_pais}
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpPut("{id_pais:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Actualizar(
            [FromRoute] int id_pais,
            [FromBody] ActualizarPaisRequest request)
        {
            var dto = new ActualizarPaisRequestDto
            {
                CodigoIso2 = request.CodigoIso2,
                CodigoIso3 = request.CodigoIso3,
                Nombre = request.Nombre,
                Continente = request.Continente
            };
            var result = await _geografiaClient.ActualizarPaisAsync(id_pais, dto);
            return result is null
                ? NotFound(new { success = false, message = "País no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Elimina un país.
        /// DELETE /api/v1/paises/{id_pais}
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpDelete("{id_pais:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar([FromRoute] int id_pais)
        {
            var result = await _geografiaClient.EliminarPaisAsync(id_pais);
            return result
                ? Ok(new { success = true, message = "País eliminado." })
                : NotFound(new { success = false, message = "País no encontrado." });
        }
    }

    // ── CiudadesController ────────────────────────────────────────────────────

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ciudades")]
    public class CiudadesController : ControllerBase
    {
        private readonly GeografiaClient _geografiaClient;

        public CiudadesController(GeografiaClient geografiaClient)
        {
            _geografiaClient = geografiaClient;
        }

        /// <summary>
        /// Lista ciudades con filtros y paginación.
        /// GET /api/v1/ciudades
        /// Público.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaged(
            [FromQuery] CiudadesFiltroRequest filtro)
        {
            var result = await _geografiaClient.GetCiudadesAsync(
                filtro.IdPais, filtro.Nombre, filtro.Estado,
                filtro.Page, filtro.PageSize);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene una ciudad por id.
        /// GET /api/v1/ciudades/{id_ciudad}
        /// Público.
        /// </summary>
        [HttpGet("{id_ciudad:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id_ciudad)
        {
            var result = await _geografiaClient.GetCiudadByIdAsync(id_ciudad);
            return result is null
                ? NotFound(new { success = false, message = "Ciudad no encontrada." })
                : Ok(new
                {
                    success = true,
                    data = new CiudadResponse
                    {
                        IdCiudad = result.IdCiudad,
                        IdPais = result.IdPais,
                        Nombre = result.Nombre,
                        CodigoPostal = result.CodigoPostal,
                        ZonaHoraria = result.ZonaHoraria,
                        Latitud = result.Latitud,
                        Longitud = result.Longitud,
                        Estado = result.Estado
                    }
                });
        }

        /// <summary>
        /// Crea una nueva ciudad.
        /// POST /api/v1/ciudades
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Crear([FromBody] CrearCiudadRequest request)
        {
            var dto = new CrearCiudadRequestDto
            {
                IdPais = request.IdPais,
                Nombre = request.Nombre,
                CodigoPostal = request.CodigoPostal,
                ZonaHoraria = request.ZonaHoraria,
                Latitud = request.Latitud,
                Longitud = request.Longitud
            };
            var result = await _geografiaClient.CrearCiudadAsync(dto);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear la ciudad." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Actualiza una ciudad.
        /// PUT /api/v1/ciudades/{id_ciudad}
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpPut("{id_ciudad:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Actualizar(
            [FromRoute] int id_ciudad,
            [FromBody] ActualizarCiudadRequest request)
        {
            var dto = new ActualizarCiudadRequestDto
            {
                IdPais = request.IdPais,
                Nombre = request.Nombre,
                CodigoPostal = request.CodigoPostal,
                ZonaHoraria = request.ZonaHoraria,
                Latitud = request.Latitud,
                Longitud = request.Longitud
            };
            var result = await _geografiaClient.ActualizarCiudadAsync(id_ciudad, dto);
            return result is null
                ? NotFound(new { success = false, message = "Ciudad no encontrada." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Elimina una ciudad.
        /// DELETE /api/v1/ciudades/{id_ciudad}
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpDelete("{id_ciudad:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar([FromRoute] int id_ciudad)
        {
            var result = await _geografiaClient.EliminarCiudadAsync(id_ciudad);
            return result
                ? Ok(new { success = true, message = "Ciudad eliminada." })
                : NotFound(new { success = false, message = "Ciudad no encontrada." });
        }
    }
}