namespace Middleware.Vuelos.Business.DTOs.Aeropuertos;

public class AeropuertosFiltroRequest
{
    public string? CodigoIata { get; set; }
    public string? CodigoIcao { get; set; }
    public string? Nombre { get; set; }
    public int? IdCiudad { get; set; }
    public int? IdPais { get; set; }
    public string? ZonaHoraria { get; set; }
    public string? Estado { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ActualizarAeropuertoRequest
{
    public string CodigoIata { get; set; } = null!;
    public string? CodigoIcao { get; set; }
    public string Nombre { get; set; } = null!;
    public int? IdCiudad { get; set; }
    public int IdPais { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
}
public class CrearAeropuertoRequest
{
    public string CodigoIata { get; set; } = null!;
    public string? CodigoIcao { get; set; }
    public string Nombre { get; set; } = null!;
    public int IdCiudad { get; set; }
    public int IdPais { get; set; }
    public string? ZonaHoraria { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
}