
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models
{
    public class VuelosAdminPagedDto<T>
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

    public class CrearVueloRequestDto
    {
        [JsonPropertyName("idAeropuertoOrigen")]
        public int IdAeropuertoOrigen { get; set; }
        [JsonPropertyName("idAeropuertoDestino")]
        public int IdAeropuertoDestino { get; set; }
        [JsonPropertyName("numeroVuelo")]
        public string NumeroVuelo { get; set; } = null!;
        [JsonPropertyName("fechaHoraSalida")]
        public DateTime FechaHoraSalida { get; set; }
        [JsonPropertyName("fechaHoraLlegada")]
        public DateTime FechaHoraLlegada { get; set; }

        [JsonPropertyName("duracionMin")]        // ✅ agregar
        public int DuracionMin { get; set; }

        [JsonPropertyName("precioBase")]
        public decimal PrecioBase { get; set; }
        [JsonPropertyName("capacidadTotal")]
        public int CapacidadTotal { get; set; }
    }

    public class ActualizarVueloRequestDto
    {
        [JsonPropertyName("idAeropuertoOrigen")]
        public int IdAeropuertoOrigen { get; set; }
        [JsonPropertyName("idAeropuertoDestino")]
        public int IdAeropuertoDestino { get; set; }
        [JsonPropertyName("numeroVuelo")]
        public string NumeroVuelo { get; set; } = null!;
        [JsonPropertyName("fechaHoraSalida")]
        public DateTime FechaHoraSalida { get; set; }
        [JsonPropertyName("fechaHoraLlegada")]
        public DateTime FechaHoraLlegada { get; set; }
        [JsonPropertyName("precioBase")]
        public decimal PrecioBase { get; set; }
        [JsonPropertyName("capacidadTotal")]
        public int CapacidadTotal { get; set; }
    }

    public class CrearEscalaRequestDto
    {
        [JsonPropertyName("idAeropuerto")]
        public int IdAeropuerto { get; set; }
        [JsonPropertyName("orden")]
        public int Orden { get; set; }
        [JsonPropertyName("fechaHoraLlegada")]
        public DateTime FechaHoraLlegada { get; set; }
        [JsonPropertyName("fechaHoraSalida")]
        public DateTime FechaHoraSalida { get; set; }
        [JsonPropertyName("tipoEscala")]
        public string TipoEscala { get; set; } = null!;
        [JsonPropertyName("terminal")]
        public string? Terminal { get; set; }
        [JsonPropertyName("puerta")]
        public string? Puerta { get; set; }
        [JsonPropertyName("observaciones")]
        public string? Observaciones { get; set; }
    }

    public class CrearAsientoRequestDto
    {
        [JsonPropertyName("numeroAsiento")]
        public string NumeroAsiento { get; set; } = null!;
        [JsonPropertyName("clase")]
        public string Clase { get; set; } = null!;
        [JsonPropertyName("precioExtra")]
        public decimal PrecioExtra { get; set; }
        [JsonPropertyName("posicion")]
        public string? Posicion { get; set; }
    }
}