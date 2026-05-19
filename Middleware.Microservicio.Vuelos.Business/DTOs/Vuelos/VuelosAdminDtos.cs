namespace Middleware.Vuelos.Business.DTOs.Vuelos;

// ── Vuelos Admin ──────────────────────────────────────────────────────────────

public class VuelosFiltroRequest
{
    public int? IdAeropuertoOrigen { get; set; }
    public int? IdAeropuertoDestino { get; set; }
    public string? NumeroVuelo { get; set; }
    public string? EstadoVuelo { get; set; }
    public DateTime? FechaSalidaDesde { get; set; }
    public DateTime? FechaSalidaHasta { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class CrearVueloRequest
{
    public int IdAeropuertoOrigen { get; set; }
    public int IdAeropuertoDestino { get; set; }
    public string NumeroVuelo { get; set; } = null!;
    public DateTime FechaHoraSalida { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public int DuracionMin { get; set; }  // ✅ agregar

    public decimal PrecioBase { get; set; }
    public int CapacidadTotal { get; set; }
}

public class ActualizarVueloRequest
{
    public int IdAeropuertoOrigen { get; set; }
    public int IdAeropuertoDestino { get; set; }
    public string NumeroVuelo { get; set; } = null!;
    public DateTime FechaHoraSalida { get; set; }
    public DateTime FechaHoraLlegada { get; set; }

    public int DuracionMin { get; set; }  // ✅ agregar

    public decimal PrecioBase { get; set; }
    public int CapacidadTotal { get; set; }
}

public class CambiarEstadoVueloRequest
{
    public string EstadoVuelo { get; set; } = null!;
}

// ── Escalas Admin ─────────────────────────────────────────────────────────────

public class CrearEscalaRequest
{
    public int IdAeropuerto { get; set; }
    public int Orden { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public DateTime FechaHoraSalida { get; set; }
    public string TipoEscala { get; set; } = null!;
    public string? Terminal { get; set; }
    public string? Puerta { get; set; }
    public string? Observaciones { get; set; }
}

// ── Asientos Admin ────────────────────────────────────────────────────────────

public class CrearAsientoRequest
{
    public string NumeroAsiento { get; set; } = null!;
    public string Clase { get; set; } = null!;
    public decimal PrecioExtra { get; set; }
    public string? Posicion { get; set; }
}

public class ActualizarAsientoRequest
{
    public bool Disponible { get; set; }
    public string? Clase { get; set; }
    public decimal? PrecioExtra { get; set; }
}