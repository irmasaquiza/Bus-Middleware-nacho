namespace Middleware.Vuelos.Business.DTOs.Booking;

/// <summary>
/// Filtros para buscar vuelos desde el portal de booking.
/// Público — no requiere JWT.
/// </summary>
public class BookingBuscarVuelosRequest
{
    public string? CodigoIataOrigen { get; set; }
    public string? CodigoIataDestino { get; set; }
    public int? IdAeropuertoOrigen { get; set; }
    public int? IdAeropuertoDestino { get; set; }
    public DateTime? FechaSalida { get; set; }
    public int? CantidadPasajeros { get; set; }
    public string? Clase { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// Filtros para buscar aeropuertos desde el portal de booking.
/// Público — no requiere JWT.
/// </summary>
public class BookingBuscarAeropuertosRequest
{
    public string? Nombre { get; set; }  // ← cambiar Search por Nombre
    public int? IdPais { get; set; }
    public int Limit { get; set; } = 10;
}

/// <summary>
/// Request para iniciar sesión de redirect a aerolínea.
/// Requiere rol BOOKING.
/// </summary>
public class BookingSessionRedirectRequest
{
    public int IdVuelo { get; set; }
    public List<int> IdAsientos { get; set; } = [];
    public string UrlRetorno { get; set; } = null!;
}

/// <summary>
/// Respuesta de la sesión de redirect.
/// Contiene el token temporal y la URL de redirect.
/// </summary>
public class BookingSessionRedirectResponse
{
    public string Token { get; set; } = null!;
    public string UrlRedirect { get; set; } = null!;
    public DateTime Expiracion { get; set; }
}

/// <summary>
/// Vista de vuelo para el portal de booking.
/// Enriquecida con datos de aeropuertos.
/// </summary>
public class BookingVueloResponse
{
    public int IdVuelo { get; set; }
    public string NumeroVuelo { get; set; } = null!;
    public int IdAeropuertoOrigen { get; set; }
    public string NombreAeropuertoOrigen { get; set; } = null!;
    public string CodigoIataOrigen { get; set; } = null!;
    public int IdAeropuertoDestino { get; set; }
    public string NombreAeropuertoDestino { get; set; } = null!;
    public string CodigoIataDestino { get; set; } = null!;
    public DateTime FechaHoraSalida { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public int DuracionMin { get; set; }
    public decimal PrecioBase { get; set; }
    public int CapacidadTotal { get; set; }
    public int AsientosDisponibles { get; set; }
    public string EstadoVuelo { get; set; } = null!;
    public List<BookingEscalaResponse> Escalas { get; set; } = [];
}

public class BookingEscalaResponse
{
    public int IdEscala { get; set; }
    public int IdAeropuerto { get; set; }
    public string NombreAeropuerto { get; set; } = null!;
    public string CodigoIata { get; set; } = null!;
    public int Orden { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public DateTime FechaHoraSalida { get; set; }
    public int DuracionMin { get; set; }
    public string TipoEscala { get; set; } = null!;
}