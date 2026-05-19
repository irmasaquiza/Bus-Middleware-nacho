
namespace Middleware.Vuelos.DataAccess.Models
{
    using System.Text.Json.Serialization;

    public class GeografiaPagedResponseDto<T>
    {
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = [];
        [JsonPropertyName("totalRegistros")]
        public int TotalRegistros { get; set; }
        [JsonPropertyName("paginaActual")]
        public int PaginaActual { get; set; }
        [JsonPropertyName("totalPaginas")]
        public int TotalPaginas { get; set; }
    }

    public class CrearPaisRequestDto
    {
        [JsonPropertyName("codigoIso2")]
        public string CodigoIso2 { get; set; } = null!;
        [JsonPropertyName("codigoIso3")]
        public string? CodigoIso3 { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = null!;
        [JsonPropertyName("continente")]
        public string? Continente { get; set; }
    }

    public class ActualizarPaisRequestDto
    {
        [JsonPropertyName("codigoIso2")]
        public string CodigoIso2 { get; set; } = null!;
        [JsonPropertyName("codigoIso3")]
        public string? CodigoIso3 { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = null!;
        [JsonPropertyName("continente")]
        public string? Continente { get; set; }
    }

    public class CrearCiudadRequestDto
    {
        [JsonPropertyName("idPais")]
        public int IdPais { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = null!;
        [JsonPropertyName("codigoPostal")]
        public string? CodigoPostal { get; set; }
        [JsonPropertyName("zonaHoraria")]
        public string? ZonaHoraria { get; set; }
        [JsonPropertyName("latitud")]
        public decimal? Latitud { get; set; }
        [JsonPropertyName("longitud")]
        public decimal? Longitud { get; set; }
    }

    public class ActualizarCiudadRequestDto
    {
        [JsonPropertyName("idPais")]
        public int IdPais { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = null!;
        [JsonPropertyName("codigoPostal")]
        public string? CodigoPostal { get; set; }
        [JsonPropertyName("zonaHoraria")]
        public string? ZonaHoraria { get; set; }
        [JsonPropertyName("latitud")]
        public decimal? Latitud { get; set; }
        [JsonPropertyName("longitud")]
        public decimal? Longitud { get; set; }
    }
}