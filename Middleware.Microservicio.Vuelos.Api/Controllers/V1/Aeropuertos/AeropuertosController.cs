using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Vuelos.Business.DTOs.Aeropuertos;
using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.Api.Controllers.V1.Aeropuertos
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/aeropuertos")]
    public class AeropuertosController : ControllerBase
    {
        private readonly AeropuertosClient _aeropuertosClient;

        public AeropuertosController(AeropuertosClient aeropuertosClient)
        {
            _aeropuertosClient = aeropuertosClient;
        }





        /// <summary>
        /// Lista aeropuertos con filtros y paginación.
        /// GET /api/v1/aeropuertos
        /// Público.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaged(
            [FromQuery] AeropuertosFiltroRequest filtro)
        {
            var result = await _aeropuertosClient.GetPagedAsync(
                filtro.CodigoIata, filtro.CodigoIcao, filtro.Nombre,
                filtro.IdCiudad, filtro.IdPais, filtro.ZonaHoraria,
                filtro.Estado, filtro.Page, filtro.PageSize);

            if (result is null)
                return Ok(new { success = true, data = result });

            // Enriquecer ciudad y pais usando los valores Raw
            foreach (var a in result.Items)
            {
                if (a.Ciudad is null && a.IdCiudadRaw.HasValue)
                    a.Ciudad = new CiudadCortoDto { IdCiudad = a.IdCiudadRaw.Value };

                if (a.Pais is null && a.IdPaisRaw.HasValue)
                    a.Pais = new PaisCortoDto { IdPais = a.IdPaisRaw.Value };
            }

            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene un aeropuerto por id.
        /// GET /api/v1/aeropuertos/{id_aeropuerto}
        /// Público.
        /// </summary>
        [HttpGet("{id_aeropuerto:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id_aeropuerto)
        {
            var result = await _aeropuertosClient.GetByIdAsync(id_aeropuerto);
            if (result is null)
                return NotFound(new { success = false, message = "Aeropuerto no encontrado." });

            if (result.Ciudad is null && result.IdCiudadRaw.HasValue)
                result.Ciudad = new CiudadCortoDto { IdCiudad = result.IdCiudadRaw.Value };

            if (result.Pais is null && result.IdPaisRaw.HasValue)
                result.Pais = new PaisCortoDto { IdPais = result.IdPaisRaw.Value };

            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene un aeropuerto por código IATA.
        /// GET /api/v1/aeropuertos/iata/{codigo}
        /// Público.
        /// </summary>
        [HttpGet("iata/{codigo}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIata([FromRoute] string codigo)
        {
            var result = await _aeropuertosClient.GetByCodigoIataAsync(codigo);
            return result is null
                ? NotFound(new { success = false, message = "Aeropuerto no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Crea un aeropuerto.
        /// POST /api/v1/aeropuertos
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear([FromBody] CrearAeropuertoRequest request)
        {
            var token = ObtenerToken();
            var dto = new CrearAeropuertoRequestDto
            {
                CodigoIata = request.CodigoIata,
                CodigoIcao = request.CodigoIcao,
                Nombre = request.Nombre,
                IdCiudad = request.IdCiudad,
                IdPais = request.IdPais,
                ZonaHoraria = request.ZonaHoraria,
                Latitud = request.Latitud,
                Longitud = request.Longitud
            };
            var result = await _aeropuertosClient.CrearAsync(dto, token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear el aeropuerto." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Actualiza un aeropuerto.
        /// PUT /api/v1/aeropuertos/{id_aeropuerto}
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPut("{id_aeropuerto:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Actualizar(
            [FromRoute] int id_aeropuerto,
            [FromBody] ActualizarAeropuertoRequest request)
        {
            var token = ObtenerToken();
            var dto = new ActualizarAeropuertoRequestDto
            {
                CodigoIata = request.CodigoIata,
                CodigoIcao = request.CodigoIcao,
                Nombre = request.Nombre,
                IdCiudad = request.IdCiudad,
                IdPais = request.IdPais,
                ZonaHoraria = request.ZonaHoraria,
                Latitud = request.Latitud,
                Longitud = request.Longitud
            };
            var result = await _aeropuertosClient.ActualizarAsync(
                id_aeropuerto, dto, token);
            return result is null
                ? NotFound(new { success = false, message = "Aeropuerto no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Elimina un aeropuerto.
        /// DELETE /api/v1/aeropuertos/{id_aeropuerto}
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpDelete("{id_aeropuerto:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar([FromRoute] int id_aeropuerto)
        {
            var token = ObtenerToken();
            var result = await _aeropuertosClient.EliminarAsync(id_aeropuerto, token);
            return result
                ? Ok(new { success = true, message = "Aeropuerto eliminado." })
                : NotFound(new { success = false, message = "Aeropuerto no encontrado." });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }
}