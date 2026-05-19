using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Vuelos.Business.Interfaces;

namespace Middleware.Vuelos.Api.Controllers.V1.PortalCliente
{
    /// <summary>
    /// Portal del Cliente — endpoints migrados desde MS Clientes al Bus.
    /// Todos requieren rol CLIENTE y extraen id_cliente del JWT automáticamente.
    /// El cliente solo puede ver SUS propios datos.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/portal/cliente")]
    [Authorize(Roles = "CLIENTE")]
    public class PortalClienteController : ControllerBase
    {
        private readonly IPortalClienteOrchestrator _portalOrchestrator;

        public PortalClienteController(IPortalClienteOrchestrator portalOrchestrator)
        {
            _portalOrchestrator = portalOrchestrator;
        }

        /// <summary>
        /// Lista todas las reservas del cliente autenticado.
        /// GET /api/v1/portal/cliente/reservas
        /// Migrado desde: GET /api/v1/cliente/reservas
        /// </summary>
        [HttpGet("reservas")]
        public async Task<IActionResult> GetMisReservas()
        {
            var (idCliente, token) = ObtenerContexto();
            var result = await _portalOrchestrator.GetMisReservasAsync(idCliente, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene el detalle de una reserva del cliente.
        /// GET /api/v1/portal/cliente/reservas/{id_reserva}/detalle
        /// Migrado desde: GET /api/v1/cliente/reservas/{id}/detalle
        /// </summary>
        [HttpGet("reservas/{id_reserva:int}/detalle")]
        public async Task<IActionResult> GetDetalleReserva([FromRoute] int id_reserva)
        {
            var (idCliente, token) = ObtenerContexto();
            var result = await _portalOrchestrator.GetDetalleReservaAsync(
                id_reserva, idCliente, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene una reserva por su código.
        /// GET /api/v1/portal/cliente/reservas/by-codigo/{codigo}
        /// Migrado desde: GET /api/v1/cliente/reservas/by-codigo/{codigo}
        /// </summary>
        [HttpGet("reservas/by-codigo/{codigo}")]
        public async Task<IActionResult> GetReservaPorCodigo([FromRoute] string codigo)
        {
            var (idCliente, token) = ObtenerContexto();
            var result = await _portalOrchestrator.GetReservaPorCodigoAsync(
                codigo, idCliente, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene la factura de una reserva del cliente.
        /// GET /api/v1/portal/cliente/reservas/{id_reserva}/factura
        /// Migrado desde: GET /api/v1/cliente/reservas/{id}/factura
        /// </summary>
        [HttpGet("reservas/{id_reserva:int}/factura")]
        public async Task<IActionResult> GetFacturaDeReserva([FromRoute] int id_reserva)
        {
            var (idCliente, token) = ObtenerContexto();
            var result = await _portalOrchestrator.GetFacturaDeReservaAsync(
                id_reserva, idCliente, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene el boleto de una reserva del cliente.
        /// GET /api/v1/portal/cliente/reservas/{id_reserva}/boleto
        /// Migrado desde: GET /api/v1/cliente/reservas/{id}/boleto
        /// </summary>
        [HttpGet("reservas/{id_reserva:int}/boleto")]
        public async Task<IActionResult> GetBoletoDeReserva([FromRoute] int id_reserva)
        {
            var (idCliente, token) = ObtenerContexto();
            var result = await _portalOrchestrator.GetBoletoDeReservaAsync(
                id_reserva, idCliente, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Lista todos los boletos del cliente.
        /// GET /api/v1/portal/cliente/boletos
        /// Migrado desde: GET /api/v1/cliente/boletos
        /// </summary>
        [HttpGet("boletos")]
        public async Task<IActionResult> GetMisBoletos()
        {
            var (idCliente, token) = ObtenerContexto();
            var result = await _portalOrchestrator.GetMisBoletosAsync(idCliente, token);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Lista todas las facturas del cliente.
        /// GET /api/v1/portal/cliente/facturas
        /// Migrado desde: GET /api/v1/cliente/facturas
        /// </summary>
        [HttpGet("facturas")]
        public async Task<IActionResult> GetMisFacturas()
        {
            var (idCliente, token) = ObtenerContexto();
            var result = await _portalOrchestrator.GetMisFacturasAsync(idCliente, token);
            return Ok(new { success = true, data = result });
        }

        // ── Helpers privados ──────────────────────────────────────────────────

        private (int idCliente, string token) ObtenerContexto()
        {
            var token = Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");

            var claim = User.Claims
                .FirstOrDefault(c => c.Type == "id_cliente")?.Value;

            if (string.IsNullOrWhiteSpace(claim) || !int.TryParse(claim, out var idCliente))
                throw new UnauthorizedAccessException(
                    "El token no contiene un id_cliente válido.");

            return (idCliente, token);
        }
    }
}

namespace Middleware.Vuelos.Api.Controllers.V1.Clientes
{
    /// <summary>
    /// Controladores de Clientes y Pasajeros.
    /// Usan MS Clientes directamente — solo reenvían el JWT.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    public class ClientesController : ControllerBase
    {
        // Los endpoints de clientes se implementan cuando el frontend los necesite.
        // Por ahora el portal del cliente usa /portal/cliente/...
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pasajeros")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    public class PasajerosController : ControllerBase
    {
        // Los endpoints de pasajeros se implementan cuando el frontend los necesite.
    }
}