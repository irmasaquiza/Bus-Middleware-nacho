
namespace Middleware.Vuelos.DataAccess.Models
{
    public class ReservasFPagedDto<T>
    {
        [System.Text.Json.Serialization.JsonPropertyName("items")]
        public List<T> Items { get; set; } = [];
        [System.Text.Json.Serialization.JsonPropertyName("totalRegistros")]
        public int TotalRegistros { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("paginaActual")]
        public int PaginaActual { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("totalPaginas")]
        public int TotalPaginas { get; set; }
    }

    public class CrearBoletoRequestDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("idReserva")]
        public int IdReserva { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("idDetalle")]
        public int IdDetalle { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("idVuelo")]
        public int IdVuelo { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("idAsiento")]
        public int IdAsiento { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("idFactura")]
        public int IdFactura { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("clase")]
        public string Clase { get; set; } = null!;
        [System.Text.Json.Serialization.JsonPropertyName("precioVueloBase")]
        public decimal PrecioVueloBase { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("precioAsientoExtra")]
        public decimal PrecioAsientoExtra { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("impuestosBoleto")]
        public decimal ImpuestosBoleto { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("cargoEquipaje")]
        public decimal CargoEquipaje { get; set; }
    }

    public class CrearFacturaRequestDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("idCliente")]
        public int IdCliente { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("idReserva")]
        public int IdReserva { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("observacionesFactura")]
        public string? ObservacionesFactura { get; set; }
    }
}