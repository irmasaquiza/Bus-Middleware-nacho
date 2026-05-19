using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Vuelos.Business.DTOs.Vuelos;
using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.Api.Controllers.V1.Vuelos
{
    // ── VuelosAdminController ─────────────────────────────────────────────────

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vuelos")]
    public class VuelosAdminController : ControllerBase
    {
        private readonly VuelosClient _vuelosClient;

        public VuelosAdminController(VuelosClient vuelosClient)
        {
            _vuelosClient = vuelosClient;
        }

        /// <summary>
        /// Lista vuelos con filtros.
        /// GET /api/v1/vuelos
        /// Público.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaged(
            [FromQuery] VuelosFiltroRequest filtro)
        {
            var result = await _vuelosClient.GetVuelosPagedAsync(
                filtro.IdAeropuertoOrigen, filtro.IdAeropuertoDestino,
                filtro.NumeroVuelo, filtro.EstadoVuelo,
                filtro.Page, filtro.PageSize);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene un vuelo por id con sus escalas.
        /// GET /api/v1/vuelos/{id_vuelo}
        /// Público.
        /// </summary>
        [HttpGet("{id_vuelo:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id_vuelo)
        {
            var result = await _vuelosClient.GetVueloByIdAsync(id_vuelo);
            return result is null
                ? NotFound(new { success = false, message = "Vuelo no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Crea un vuelo.
        /// POST /api/v1/vuelos
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear([FromBody] CrearVueloRequest request)
        {
            var token = ObtenerToken();
            var dto = new CrearVueloRequestDto
            {
                IdAeropuertoOrigen = request.IdAeropuertoOrigen,
                IdAeropuertoDestino = request.IdAeropuertoDestino,
                NumeroVuelo = request.NumeroVuelo,
                FechaHoraSalida = request.FechaHoraSalida,
                FechaHoraLlegada = request.FechaHoraLlegada,
                DuracionMin = request.DuracionMin,  // ✅ agregar esto
                PrecioBase = request.PrecioBase,
                CapacidadTotal = request.CapacidadTotal
            };
            var result = await _vuelosClient.CrearVueloAsync(dto, token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear el vuelo." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Actualiza un vuelo.
        /// PUT /api/v1/vuelos/{id_vuelo}
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPut("{id_vuelo:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Actualizar(
            [FromRoute] int id_vuelo,
            [FromBody] ActualizarVueloRequest request)
        {
            var token = ObtenerToken();
            var dto = new ActualizarVueloRequestDto
            {
                IdAeropuertoOrigen = request.IdAeropuertoOrigen,
                IdAeropuertoDestino = request.IdAeropuertoDestino,
                NumeroVuelo = request.NumeroVuelo,
                FechaHoraSalida = request.FechaHoraSalida,
                FechaHoraLlegada = request.FechaHoraLlegada,
                PrecioBase = request.PrecioBase,
                CapacidadTotal = request.CapacidadTotal
            };
            var result = await _vuelosClient.ActualizarVueloAsync(id_vuelo, dto, token);
            return result is null
                ? NotFound(new { success = false, message = "Vuelo no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Cambia el estado de un vuelo.
        /// PATCH /api/v1/vuelos/{id_vuelo}/estado
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPatch("{id_vuelo:int}/estado")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> CambiarEstado(
            [FromRoute] int id_vuelo,
            [FromBody] CambiarEstadoVueloRequest request)
        {
            var token = ObtenerToken();
            var result = await _vuelosClient.CambiarEstadoVueloAsync(
                id_vuelo, request.EstadoVuelo, token);
            return result is null
                ? NotFound(new { success = false, message = "Vuelo no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Elimina un vuelo.
        /// DELETE /api/v1/vuelos/{id_vuelo}
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpDelete("{id_vuelo:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar([FromRoute] int id_vuelo)
        {
            var token = ObtenerToken();
            var result = await _vuelosClient.EliminarVueloAsync(id_vuelo, token);
            return result
                ? Ok(new { success = true, message = "Vuelo eliminado." })
                : NotFound(new { success = false, message = "Vuelo no encontrado." });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }

    // ── EscalasAdminController ────────────────────────────────────────────────

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vuelos/{id_vuelo:int}/escalas")]
    public class EscalasAdminController : ControllerBase
    {
        private readonly VuelosClient _vuelosClient;

        public EscalasAdminController(VuelosClient vuelosClient)
        {
            _vuelosClient = vuelosClient;
        }

        /// <summary>
        /// Lista escalas de un vuelo.
        /// GET /api/v1/vuelos/{id_vuelo}/escalas
        /// Público.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetEscalas([FromRoute] int id_vuelo)
        {
            var result = await _vuelosClient.GetEscalasByVueloAsync(id_vuelo);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene una escala específica.
        /// GET /api/v1/vuelos/{id_vuelo}/escalas/{id_escala}
        /// Público.
        /// </summary>
        [HttpGet("{id_escala:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(
            [FromRoute] int id_vuelo, [FromRoute] int id_escala)
        {
            var result = await _vuelosClient.GetEscalaByIdAsync(id_vuelo, id_escala);
            return result is null
                ? NotFound(new { success = false, message = "Escala no encontrada." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Crea una escala en un vuelo.
        /// POST /api/v1/vuelos/{id_vuelo}/escalas
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear(
            [FromRoute] int id_vuelo,
            [FromBody] CrearEscalaRequest request)
        {
            var token = ObtenerToken();
            var dto = new CrearEscalaRequestDto
            {
                IdAeropuerto = request.IdAeropuerto,
                Orden = request.Orden,
                FechaHoraLlegada = request.FechaHoraLlegada,
                FechaHoraSalida = request.FechaHoraSalida,
                TipoEscala = request.TipoEscala,
                Terminal = request.Terminal,
                Puerta = request.Puerta,
                Observaciones = request.Observaciones
            };
            var result = await _vuelosClient.CrearEscalaAsync(id_vuelo, dto, token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear la escala." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Elimina una escala.
        /// DELETE /api/v1/vuelos/{id_vuelo}/escalas/{id_escala}
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpDelete("{id_escala:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Eliminar(
            [FromRoute] int id_vuelo, [FromRoute] int id_escala)
        {
            var token = ObtenerToken();
            var result = await _vuelosClient.EliminarEscalaAsync(id_vuelo, id_escala, token);
            return result
                ? Ok(new { success = true, message = "Escala eliminada." })
                : NotFound(new { success = false, message = "Escala no encontrada." });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }

    // ── AsientosAdminController ───────────────────────────────────────────────

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vuelos/{id_vuelo:int}/asientos")]
    public class AsientosAdminController : ControllerBase
    {
        private readonly VuelosClient _vuelosClient;

        public AsientosAdminController(VuelosClient vuelosClient)
        {
            _vuelosClient = vuelosClient;
        }

        /// <summary>
        /// Lista asientos de un vuelo.
        /// GET /api/v1/vuelos/{id_vuelo}/asientos
        /// Público.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsientos([FromRoute] int id_vuelo)
        {
            var result = await _vuelosClient.GetAsientosByVueloAsync(id_vuelo);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene un asiento específico.
        /// GET /api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
        /// Público.
        /// </summary>
        [HttpGet("{id_asiento:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(
            [FromRoute] int id_vuelo, [FromRoute] int id_asiento)
        {
            var result = await _vuelosClient
                .GetAsientoByIdAdminAsync(id_vuelo, id_asiento);
            return result is null
                ? NotFound(new { success = false, message = "Asiento no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Crea un asiento en un vuelo.
        /// POST /api/v1/vuelos/{id_vuelo}/asientos
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear(
            [FromRoute] int id_vuelo,
            [FromBody] CrearAsientoRequest request)
        {
            var token = ObtenerToken();
            var dto = new CrearAsientoRequestDto
            {
                NumeroAsiento = request.NumeroAsiento,
                Clase = request.Clase,
                PrecioExtra = request.PrecioExtra,
                Posicion = request.Posicion
            };
            var result = await _vuelosClient.CrearAsientoAsync(id_vuelo, dto, token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear el asiento." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Actualiza disponibilidad de un asiento.
        /// PATCH /api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPatch("{id_asiento:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> ActualizarDisponibilidad(
            [FromRoute] int id_vuelo,
            [FromRoute] int id_asiento,
            [FromBody] ActualizarAsientoRequest request)
        {
            var token = ObtenerToken();
            var result = await _vuelosClient.ActualizarDisponibilidadAsientoAsync(
                id_vuelo, id_asiento, request.Disponible, token);
            return result is null
                ? NotFound(new { success = false, message = "Asiento no encontrado." })
                : Ok(new { success = true, data = result });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }
}