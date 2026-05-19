using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Vuelos.Business.DTOs.Reservas;
using Middleware.Vuelos.Business.Interfaces;
using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Models;

namespace Middleware.Vuelos.Api.Controllers.V1.Reservas
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/boletos")]
    [Authorize]
    public class BoletosController : ControllerBase
    {
        private readonly IReservaOrchestrator _reservaOrchestrator;
        private readonly ReservasFClient _reservasFClient;

        public BoletosController(
            IReservaOrchestrator reservaOrchestrator,
            ReservasFClient reservasFClient)
        {
            _reservaOrchestrator = reservaOrchestrator;
            _reservasFClient = reservasFClient;
        }

        /// <summary>
        /// Lista boletos con filtros.
        /// GET /api/v1/boletos
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] BoletosFiltroRequest filtro)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.GetBoletosPagedAsync(
                filtro.IdReserva, filtro.IdVuelo, filtro.CodigoBoleto,
                filtro.EstadoBoleto, filtro.Page, filtro.PageSize, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene los boletos de una reserva.
        /// GET /api/v1/boletos?idReserva={id}
        /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
        /// </summary>
        [HttpGet("por-reserva")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
        public async Task<IActionResult> GetByReserva([FromQuery] int idReserva)
        {
            var token = ObtenerToken();
            var result = await _reservaOrchestrator.GetBoletosByReservaAsync(
                idReserva, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene un boleto por id.
        /// GET /api/v1/boletos/{id_boleto}
        /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
        /// </summary>
        [HttpGet("{id_boleto:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
        public async Task<IActionResult> GetById([FromRoute] int id_boleto)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.GetBoletoByIdAsync(id_boleto, token);
            return result is null
                ? NotFound(new { success = false, message = "Boleto no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Crea un boleto manualmente.
        /// POST /api/v1/boletos
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear([FromBody] CrearBoletoRequest request)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.CrearBoletoAsync(
                new CrearBoletoRequestDto
                {
                    IdReserva = request.IdReserva,
                    IdDetalle = request.IdDetalle,
                    IdVuelo = request.IdVuelo,
                    IdAsiento = request.IdAsiento,
                    IdFactura = request.IdFactura,
                    Clase = request.Clase,
                    PrecioVueloBase = request.PrecioVueloBase,
                    PrecioAsientoExtra = request.PrecioAsientoExtra,
                    ImpuestosBoleto = request.ImpuestosBoleto,
                    CargoEquipaje = request.CargoEquipaje
                },
                token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear el boleto." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Cambia el estado de un boleto.
        /// PATCH /api/v1/boletos/{id_boleto}/estado
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPatch("{id_boleto:int}/estado")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> CambiarEstado(
            [FromRoute] int id_boleto,
            [FromBody] CambiarEstadoBoletoRequest request)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.CambiarEstadoBoletoAsync(
                id_boleto, request.EstadoBoleto, token);
            return result is null
                ? NotFound(new { success = false, message = "Boleto no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Elimina un boleto.
        /// DELETE /api/v1/boletos/{id_boleto}
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpDelete("{id_boleto:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Eliminar([FromRoute] int id_boleto)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.EliminarBoletoAsync(id_boleto, token);
            return result
                ? Ok(new { success = true, message = "Boleto eliminado." })
                : NotFound(new { success = false, message = "Boleto no encontrado." });
        }

        /// <summary>
        /// Lista el equipaje de un boleto.
        /// GET /api/v1/boletos/{id_boleto}/equipaje
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpGet("{id_boleto:int}/equipaje")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetEquipaje([FromRoute] int id_boleto)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.GetEquipajeByBoletoAsync(id_boleto, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Agrega equipaje a un boleto.
        /// POST /api/v1/boletos/{id_boleto}/equipaje
        /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
        /// </summary>
        [HttpPost("{id_boleto:int}/equipaje")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
        public async Task<IActionResult> AgregarEquipaje(
            [FromRoute] int id_boleto,
            [FromBody] AgregarEquipajeAdminRequest request)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.AgregarEquipajeAsync(
                id_boleto,
                new AgregarEquipajeRequestDto
                {
                    Tipo = request.Tipo,
                    PesoKg = request.PesoKg,
                    DescripcionEquipaje = request.DescripcionEquipaje,
                    DimensionesCm = request.DimensionesCm
                },
                token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo agregar el equipaje." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Cambia el estado del equipaje.
        /// PATCH /api/v1/boletos/{id_boleto}/equipaje/{id_equipaje}/estado
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPatch("{id_boleto:int}/equipaje/{id_equipaje:int}/estado")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> CambiarEstadoEquipaje(
            [FromRoute] int id_boleto,
            [FromRoute] int id_equipaje,
            [FromBody] CambiarEstadoEquipajeRequest request)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.CambiarEstadoEquipajeAsync(
                id_boleto, id_equipaje, request.EstadoEquipaje, token);
            return result is null
                ? NotFound(new { success = false, message = "Equipaje no encontrado." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Elimina un equipaje.
        /// DELETE /api/v1/boletos/{id_boleto}/equipaje/{id_equipaje}
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpDelete("{id_boleto:int}/equipaje/{id_equipaje:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> EliminarEquipaje(
            [FromRoute] int id_boleto,
            [FromRoute] int id_equipaje)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.EliminarEquipajeAsync(
                id_boleto, id_equipaje, token);
            return result
                ? Ok(new { success = true, message = "Equipaje eliminado." })
                : NotFound(new { success = false, message = "Equipaje no encontrado." });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }
}