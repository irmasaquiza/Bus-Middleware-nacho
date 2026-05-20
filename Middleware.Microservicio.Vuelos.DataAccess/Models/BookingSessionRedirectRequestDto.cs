using Middleware.Vuelos.DataAccess.Clients;
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models
{
    public class BookingSessionRedirectRequestDto
    {
        [JsonPropertyName("idVuelo")]
        public int IdVuelo { get; set; }

        [JsonPropertyName("urlRetorno")]
        public string UrlRetorno { get; set; } = null!;
    }

    public class BookingSessionRedirectResponseDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;

        [JsonPropertyName("urlRedirect")]
        public string UrlRedirect { get; set; } = null!;

        [JsonPropertyName("expiracion")]
        public DateTime Expiracion { get; set; }
    }

    // ← Agregar estos dos al final
    public class VuelosBookingResponseDto
    {
        [JsonPropertyName("meta")]
        public VuelosBookingMetaDto? Meta { get; set; }

        [JsonPropertyName("data")]
        public List<BookingVueloItemDto>? Data { get; set; }
    }

    public class VuelosBookingMetaDto
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("moneda")]
        public string Moneda { get; set; } = "USD";
    }
}