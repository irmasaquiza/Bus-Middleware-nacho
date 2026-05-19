
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models
{
    public class AeropuertosPagedResponseDto<T>
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

    public class ActualizarAeropuertoRequestDto
    {
        [JsonPropertyName("codigoIata")]
        public string CodigoIata { get; set; } = null!;
        [JsonPropertyName("codigoIcao")]
        public string? CodigoIcao { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = null!;
        [JsonPropertyName("idCiudad")]
        public int? IdCiudad { get; set; }
        [JsonPropertyName("idPais")]
        public int IdPais { get; set; }
        [JsonPropertyName("zonaHoraria")]
        public string? ZonaHoraria { get; set; }
        [JsonPropertyName("latitud")]
        public decimal? Latitud { get; set; }
        [JsonPropertyName("longitud")]
        public decimal? Longitud { get; set; }
    }
}