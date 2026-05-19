namespace Middleware.Vuelos.Business.DTOs.Reservas;

// ── Filtros Admin ─────────────────────────────────────────────────────────────

public class ReservasFiltroRequest
{
    public int? IdCliente { get; set; }
    public int? IdVuelo { get; set; }
    public string? CodigoReserva { get; set; }
    public string? EstadoReserva { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class BoletosFiltroRequest
{
    public int? IdReserva { get; set; }
    public int? IdVuelo { get; set; }
    public string? CodigoBoleto { get; set; }
    public string? EstadoBoleto { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class FacturasFiltroRequest
{
    public int? IdCliente { get; set; }
    public int? IdReserva { get; set; }
    public string? NumeroFactura { get; set; }
    public string? EstadoFactura { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

// ── Reservas Admin ────────────────────────────────────────────────────────────

public class CambiarEstadoReservaRequest
{
    public string EstadoReserva { get; set; } = null!;
    public string? MotivoCancelacion { get; set; }
}

// ── Boletos Admin ─────────────────────────────────────────────────────────────

public class CrearBoletoRequest
{
    public int IdReserva { get; set; }
    public int IdDetalle { get; set; }
    public int IdVuelo { get; set; }
    public int IdAsiento { get; set; }
    public int IdFactura { get; set; }
    public string Clase { get; set; } = null!;
    public decimal PrecioVueloBase { get; set; }
    public decimal PrecioAsientoExtra { get; set; }
    public decimal ImpuestosBoleto { get; set; }
    public decimal CargoEquipaje { get; set; }
}

public class CambiarEstadoBoletoRequest
{
    public string EstadoBoleto { get; set; } = null!;
}

// ── Facturas Admin ────────────────────────────────────────────────────────────

public class CrearFacturaRequest
{
    public int IdCliente { get; set; }
    public int IdReserva { get; set; }
    public string? ObservacionesFactura { get; set; }
}

// ── Equipaje Admin ────────────────────────────────────────────────────────────

public class AgregarEquipajeAdminRequest
{
    public string Tipo { get; set; } = null!;
    public decimal PesoKg { get; set; }
    public string? DescripcionEquipaje { get; set; }
    public string? DimensionesCm { get; set; }
}

public class CambiarEstadoEquipajeRequest
{
    public string EstadoEquipaje { get; set; } = null!;
}