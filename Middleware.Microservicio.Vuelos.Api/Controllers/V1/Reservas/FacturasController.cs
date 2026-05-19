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
    [Route("api/v{version:apiVersion}/facturas")]
    [Authorize]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturacionOrchestrator _facturacionOrchestrator;
        private readonly ReservasFClient _reservasFClient;

        public FacturasController(
            IFacturacionOrchestrator facturacionOrchestrator,
            ReservasFClient reservasFClient)
        {
            _facturacionOrchestrator = facturacionOrchestrator;
            _reservasFClient = reservasFClient;
        }

        /// <summary>
        /// Lista facturas con filtros.
        /// GET /api/v1/facturas
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] FacturasFiltroRequest filtro)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.GetFacturasPagedAsync(
                filtro.IdCliente, filtro.IdReserva, filtro.NumeroFactura,
                filtro.EstadoFactura, filtro.Page, filtro.PageSize, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene una factura por id.
        /// GET /api/v1/facturas/{id_factura}
        /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
        /// </summary>
        [HttpGet("{id_factura:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
        public async Task<IActionResult> GetById([FromRoute] int id_factura)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.GetFacturaByIdAsync(id_factura, token);
            return result is null
                ? NotFound(new { success = false, message = "Factura no encontrada." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene la factura de una reserva.
        /// GET /api/v1/facturas/reserva/{id_reserva}
        /// Roles: ADMINISTRADOR, AEROLINEA, CLIENTE
        /// </summary>
        [HttpGet("reserva/{id_reserva:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
        public async Task<IActionResult> GetByReserva([FromRoute] int id_reserva)
        {
            var token = ObtenerToken();
            var result = await _facturacionOrchestrator.GetFacturaByReservaAsync(
                id_reserva, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Crea una factura manualmente.
        /// POST /api/v1/facturas
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear([FromBody] CrearFacturaRequest request)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.CrearFacturaAsync(
                new CrearFacturaRequestDto
                {
                    IdCliente = request.IdCliente,
                    IdReserva = request.IdReserva,
                    ObservacionesFactura = request.ObservacionesFactura
                },
                token);
            return result is null
                ? BadRequest(new { success = false, message = "No se pudo crear la factura." })
                : Created(string.Empty, new { success = true, data = result });
        }

        /// <summary>
        /// Anula una factura.
        /// PATCH /api/v1/facturas/{id_factura}/anular
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpPatch("{id_factura:int}/anular")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Anular([FromRoute] int id_factura)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.AnularFacturaAsync(id_factura, token);
            return result is null
                ? NotFound(new { success = false, message = "Factura no encontrada." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Aprueba una factura.
        /// PATCH /api/v1/facturas/{id_factura}/aprobar
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPatch("{id_factura:int}/aprobar")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Aprobar([FromRoute] int id_factura)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.AprobarFacturaAsync(id_factura, token);
            return result is null
                ? NotFound(new { success = false, message = "Factura no encontrada." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Procesa el pago de una factura.
        /// POST /api/v1/facturas/{id_factura}/pagar
        /// Roles: ADMINISTRADOR, AEROLINEA
        /// </summary>
        [HttpPost("{id_factura:int}/pagar")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Pagar([FromRoute] int id_factura)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.PagarFacturaAsync(id_factura, token);
            return result is null
                ? NotFound(new { success = false, message = "Factura no encontrada." })
                : Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Elimina una factura.
        /// DELETE /api/v1/facturas/{id_factura}
        /// Rol: ADMINISTRADOR
        /// </summary>
        [HttpDelete("{id_factura:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar([FromRoute] int id_factura)
        {
            var token = ObtenerToken();
            var result = await _reservasFClient.EliminarFacturaAsync(id_factura, token);
            return result
                ? Ok(new { success = true, message = "Factura eliminada." })
                : NotFound(new { success = false, message = "Factura no encontrada." });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }
}