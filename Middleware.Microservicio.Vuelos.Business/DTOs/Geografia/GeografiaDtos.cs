namespace Middleware.Vuelos.Business.DTOs.Geografia;

// ── País ──────────────────────────────────────────────────────────────────────

public class PaisResponse
{
    public int IdPais { get; set; }
    public string CodigoIso2 { get; set; } = null!;
    public string? CodigoIso3 { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Continente { get; set; }
    public string Estado { get; set; } = null!;
}

public class CrearPaisRequest
{
    public string CodigoIso2 { get; set; } = null!;
    public string? CodigoIso3 { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Continente { get; set; }
}

public class ActualizarPaisRequest
{
    public string CodigoIso2 { get; set; } = null!;
    public string? CodigoIso3 { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Continente { get; set; }
}

// ── Ciudad ────────────────────────────────────────────────────────────────────

public class CiudadResponse
{
    public int IdCiudad { get; set; }
    public int IdPais { get; set; }
    public string Nombre { get; set; } = null!;
    public string? CodigoPostal { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string Estado { get; set; } = null!;
}

public class CrearCiudadRequest
{
    public int IdPais { get; set; }
    public string Nombre { get; set; } = null!;
    public string? CodigoPostal { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
}

public class ActualizarCiudadRequest
{
    public int IdPais { get; set; }
    public string Nombre { get; set; } = null!;
    public string? CodigoPostal { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
}

// ── Filtros ───────────────────────────────────────────────────────────────────

public class PaisesFiltroRequest
{
    public string? Nombre { get; set; }
    public string? CodigoIso2 { get; set; }
    public string? CodigoIso3 { get; set; }
    public string? Continente { get; set; }
    public string? Estado { get; set; }
    public int PaginaActual { get; set; } = 1;
    public int TamanoPagina { get; set; } = 20;
}

public class CiudadesFiltroRequest
{
    public int? IdPais { get; set; }
    public string? Nombre { get; set; }
    public string? CodigoPostal { get; set; }
    public string? ZonaHoraria { get; set; }
    public string? Estado { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}