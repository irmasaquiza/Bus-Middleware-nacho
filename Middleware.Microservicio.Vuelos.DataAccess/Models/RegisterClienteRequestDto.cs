
// ── DTOs nuevos de Seguridad ──────────────────────────────────────────────────

namespace Middleware.Vuelos.DataAccess.Models
{
    using System.Text.Json.Serialization;

    public class RegisterClienteRequestDto
    {
        [JsonPropertyName("tipo_identificacion")]
        public string TipoIdentificacion { get; set; } = null!;
        [JsonPropertyName("numero_identificacion")]
        public string NumeroIdentificacion { get; set; } = null!;
        [JsonPropertyName("nombres")]
        public string Nombres { get; set; } = null!;
        [JsonPropertyName("apellidos")]
        public string? Apellidos { get; set; }
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
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
        [JsonPropertyName("id_cliente")]
        public int? IdCliente { get; set; }
    }

    public class RegisterClienteResponseDto
    {
        [JsonPropertyName("idCliente")]
        public int IdCliente { get; set; }
        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;
        [JsonPropertyName("rolAsignado")]
        public string RolAsignado { get; set; } = null!;
    }

    public class UsuarioResponseDto
    {
        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }
        [JsonPropertyName("usuarioGuid")]
        public Guid UsuarioGuid { get; set; }
        [JsonPropertyName("idCliente")]
        public int? IdCliente { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;
        [JsonPropertyName("correo")]
        public string Correo { get; set; } = null!;
        [JsonPropertyName("estadoUsuario")]
        public string EstadoUsuario { get; set; } = null!;
        [JsonPropertyName("activo")]
        public bool Activo { get; set; }
    }

    public class CrearUsuarioRequestDto
    {
        [JsonPropertyName("id_cliente")]
        public int? IdCliente { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;
        [JsonPropertyName("correo")]
        public string Correo { get; set; } = null!;
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }

    public class ActualizarUsuarioRequestDto
    {
        [JsonPropertyName("correo")]
        public string Correo { get; set; } = null!;
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }

    public class AsignarRolRequestDto
    {
        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }
        [JsonPropertyName("idRol")]
        public int IdRol { get; set; }
    }

    public class UsuarioRolResponseDto
    {
        [JsonPropertyName("idUsuarioRol")]
        public int IdUsuarioRol { get; set; }
        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }
        [JsonPropertyName("idRol")]
        public int IdRol { get; set; }
    }

    public class SeguridadPagedResponseDto<T>
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
}