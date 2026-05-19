using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Vuelos.Business.DTOs.Booking;
using Middleware.Vuelos.Business.Exceptions;
using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Models;
using Middleware.Vuelos.DataManagement.Interfaces;

namespace Middleware.Vuelos.Api.Controllers.V1.Booking
{
    /// <summary>
    /// Booking Controller — Portal público de búsqueda y reserva de vuelos.
    /// Los endpoints de consulta son públicos.
    /// La sesión de redirect requiere rol BOOKING.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/booking")]
    public class BookingController : ControllerBase
    {
        private readonly VuelosClient _vuelosClient;
        private readonly IAeropuertosDataService _aeropuertosDataService;

        public BookingController(
            VuelosClient vuelosClient,
            IAeropuertosDataService aeropuertosDataService)
        {
            _vuelosClient = vuelosClient;
            _aeropuertosDataService = aeropuertosDataService;
        }

        /// <summary>
        /// Busca vuelos disponibles con filtros.
        /// GET /api/v1/booking/vuelos/buscar
        /// Público — no requiere JWT.
        /// </summary>
        [HttpGet("vuelos/buscar")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarVuelos(
            [FromQuery] BookingBuscarVuelosRequest request)
        {
            // Si vienen IDs pero no códigos IATA, resolverlos desde MS Aeropuertos
            var codigoOrigen = request.CodigoIataOrigen;
            var codigoDestino = request.CodigoIataDestino;

            if (string.IsNullOrWhiteSpace(codigoOrigen) &&
                request.IdAeropuertoOrigen.HasValue &&
                request.IdAeropuertoOrigen.Value > 0)  // ← agregar este check
            {
                var aero = await _aeropuertosDataService.GetByIdAsync(
                    request.IdAeropuertoOrigen.Value);
                codigoOrigen = aero?.CodigoIata;
            }

            if (string.IsNullOrWhiteSpace(codigoDestino) &&
                request.IdAeropuertoDestino.HasValue &&
                request.IdAeropuertoDestino.Value > 0)  // ← agregar este check
            {
                var aero = await _aeropuertosDataService.GetByIdAsync(
                    request.IdAeropuertoDestino.Value);
                codigoDestino = aero?.CodigoIata;
            }

            var result = await _vuelosClient.BookingBuscarVuelosAsync(
                codigoOrigen,
                codigoDestino,
                request.IdAeropuertoOrigen,
                request.IdAeropuertoDestino,
                request.FechaSalida,
                request.CantidadPasajeros,
                request.Clase,
                request.Page,
                request.PageSize);

            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene el detalle de un vuelo para booking.
        /// Enriquece con nombres de aeropuertos.
        /// GET /api/v1/booking/vuelos/{id_vuelo}
        /// Público — no requiere JWT.
        /// </summary>
        [HttpGet("vuelos/{id_vuelo:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVuelo([FromRoute] int id_vuelo)
        {
            var vuelo = await _vuelosClient.BookingGetVueloByIdAsync(id_vuelo);
            if (vuelo is null)
                return NotFound(new { success = false, message = "Vuelo no encontrado." });

            // Enriquecer con nombres de aeropuertos
            var aeroOrigen = await _aeropuertosDataService.GetByIdAsync(vuelo.IdAeropuertoOrigen);
            var aeroDest = await _aeropuertosDataService.GetByIdAsync(vuelo.IdAeropuertoDestino);
            var escalas = await _vuelosClient.BookingGetEscalasAsync(id_vuelo);

            var response = new BookingVueloResponse
            {
                IdVuelo = vuelo.IdVuelo,
                NumeroVuelo = vuelo.NumeroVuelo,
                IdAeropuertoOrigen = vuelo.IdAeropuertoOrigen,
                NombreAeropuertoOrigen = aeroOrigen?.Nombre ?? string.Empty,
                CodigoIataOrigen = aeroOrigen?.CodigoIata ?? string.Empty,
                IdAeropuertoDestino = vuelo.IdAeropuertoDestino,
                NombreAeropuertoDestino = aeroDest?.Nombre ?? string.Empty,
                CodigoIataDestino = aeroDest?.CodigoIata ?? string.Empty,
                FechaHoraSalida = vuelo.FechaHoraSalida,
                FechaHoraLlegada = vuelo.FechaHoraLlegada,
                DuracionMin = vuelo.DuracionMin,
                PrecioBase = vuelo.PrecioBase,
                CapacidadTotal = vuelo.CapacidadTotal,
                EstadoVuelo = vuelo.EstadoVuelo,
                Escalas = escalas.Select(e => new BookingEscalaResponse
                {
                    IdEscala = e.IdEscala,
                    IdAeropuerto = e.IdAeropuerto,
                    NombreAeropuerto = string.Empty, // enriquecer si se necesita
                    CodigoIata = string.Empty,
                    Orden = e.Orden,
                    FechaHoraLlegada = e.FechaHoraLlegada,
                    FechaHoraSalida = e.FechaHoraSalida,
                    DuracionMin = e.DuracionMin,
                    TipoEscala = e.TipoEscala
                }).ToList()
            };

            return Ok(new { success = true, data = response });
        }

        /// <summary>
        /// Obtiene las escalas de un vuelo para booking.
        /// GET /api/v1/booking/vuelos/{id_vuelo}/escalas
        /// Público — no requiere JWT.
        /// </summary>
        [HttpGet("vuelos/{id_vuelo:int}/escalas")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEscalas([FromRoute] int id_vuelo)
        {
            var result = await _vuelosClient.BookingGetEscalasAsync(id_vuelo);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Obtiene los asientos disponibles de un vuelo para booking.
        /// GET /api/v1/booking/vuelos/{id_vuelo}/asientos
        /// Público — no requiere JWT.
        /// </summary>
        [HttpGet("vuelos/{id_vuelo:int}/asientos")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsientos([FromRoute] int id_vuelo)
        {
            var result = await _vuelosClient.BookingGetAsientosAsync(id_vuelo);
            // Solo devolver asientos disponibles al portal de booking
            var disponibles = result.Where(a => a.Disponible && !a.Eliminado).ToList();
            return Ok(new { success = true, data = disponibles });
        }

        /// <summary>
        /// Busca aeropuertos para el selector de búsqueda de vuelos.
        /// GET /api/v1/booking/aeropuertos
        /// Público — no requiere JWT.
        /// </summary>
        [HttpGet("aeropuertos")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarAeropuertos(
            [FromQuery] BookingBuscarAeropuertosRequest request)
        {
            var result = await _vuelosClient.BookingBuscarAeropuertosAsync(
                request.Search, request.IdPais, request.Limit);
            return Ok(new { success = true, data = result });
        }

        /// <summary>
        /// Inicia sesión de redirect a aerolínea.
        /// POST /api/v1/booking/vuelos/sesion-redirect
        /// Rol: BOOKING
        /// </summary>
        [HttpPost("vuelos/sesion-redirect")]
        [Authorize(Roles = "BOOKING")]
        public async Task<IActionResult> SesionRedirect(
            [FromBody] BookingSessionRedirectRequest request)
        {
            var token = ObtenerToken();
            var dto = new BookingSessionRedirectRequestDto
            {
                IdVuelo = request.IdVuelo,
                IdAsientos = request.IdAsientos,
                UrlRetorno = request.UrlRetorno
            };

            var result = await _vuelosClient.BookingSessionRedirectAsync(dto, token);

            if (result is null)
                return BadRequest(new
                {
                    success = false,
                    message = "No se pudo iniciar la sesión de redirect."
                });

            return Ok(new
            {
                success = true,
                data = new BookingSessionRedirectResponse
                {
                    Token = result.Token,
                    UrlRedirect = result.UrlRedirect,
                    Expiracion = result.Expiracion
                }
            });
        }

        private string ObtenerToken() =>
            Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");
    }
}