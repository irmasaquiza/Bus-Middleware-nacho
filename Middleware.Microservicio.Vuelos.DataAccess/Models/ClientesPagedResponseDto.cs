
using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models
{
    public class ClientesPagedResponseDto<T>
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

    public class ActualizarClienteRequestDto
    {
        [JsonPropertyName("nombres")]
        public string Nombres { get; set; } = null!;
        [JsonPropertyName("apellidos")]
        public string? Apellidos { get; set; }
        [JsonPropertyName("razon_social")]
        public string? RazonSocial { get; set; }
        [JsonPropertyName("correo")]
        public string Correo { get; set; } = null!;
        [JsonPropertyName("telefono")]
        public string Telefono { get; set; } = null!;
        [JsonPropertyName("direccion")]
        public string Direccion { get; set; } = null!;
        [JsonPropertyName("id_ciudad_residencia")]
        public int IdCiudadResidencia { get; set; }
        [JsonPropertyName("id_pais_nacionalidad")]
        public int IdPaisNacionalidad { get; set; }
        [JsonPropertyName("fecha_nacimiento")]
        public DateTime? FechaNacimiento { get; set; }
        [JsonPropertyName("genero")]
        public string? Genero { get; set; }
    }

    public class ActualizarPasajeroRequestDto
    {
        [JsonPropertyName("nombre_pasajero")]
        public string NombrePasajero { get; set; } = null!;
        [JsonPropertyName("apellido_pasajero")]
        public string ApellidoPasajero { get; set; } = null!;
        [JsonPropertyName("tipo_documento_pasajero")]
        public string TipoDocumentoPasajero { get; set; } = null!;
        [JsonPropertyName("numero_documento_pasajero")]
        public string NumeroDocumentoPasajero { get; set; } = null!;
        [JsonPropertyName("fecha_nacimiento_pasajero")]
        public DateTime? FechaNacimientoPasajero { get; set; }
        [JsonPropertyName("id_pais_nacionalidad")]
        public int? IdPaisNacionalidad { get; set; }
        [JsonPropertyName("email_contacto_pasajero")]
        public string? EmailContactoPasajero { get; set; }
        [JsonPropertyName("telefono_contacto_pasajero")]
        public string? TelefonoContactoPasajero { get; set; }
        [JsonPropertyName("genero_pasajero")]
        public string? GeneroPasajero { get; set; }
        [JsonPropertyName("requiere_asistencia")]
        public bool RequiereAsistencia { get; set; }
        [JsonPropertyName("observaciones_pasajero")]
        public string? ObservacionesPasajero { get; set; }
    }
}