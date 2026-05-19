namespace Middleware.Vuelos.Business.DTOs.Seguridad;

// ── Usuarios Admin ────────────────────────────────────────────────────────────

public class UsuarioResponse
{
    public int IdUsuario { get; set; }
    public Guid UsuarioGuid { get; set; }
    public int? IdCliente { get; set; }
    public string Username { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string EstadoUsuario { get; set; } = null!;
    public bool Activo { get; set; }
    public DateTime? FechaUltimoLogin { get; set; }
    public List<string> Roles { get; set; } = [];
}

public class CrearUsuarioRequest
{
    public int? IdCliente { get; set; }
    public string Username { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class ActualizarUsuarioRequest
{
    public string Correo { get; set; } = null!;
    public string? Password { get; set; }
}

// ── Roles Admin ───────────────────────────────────────────────────────────────

public class AsignarRolRequest
{
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
}

public class UsuarioRolResponse
{
    public int IdUsuarioRol { get; set; }
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
    public string NombreRol { get; set; } = null!;
    public string EstadoUsuarioRol { get; set; } = null!;
}

// ── Register + Logout ─────────────────────────────────────────────────────────

public class RegisterClienteRequest
{
    public string TipoIdentificacion { get; set; } = null!;
    public string NumeroIdentificacion { get; set; } = null!;
    public string Nombres { get; set; } = null!;
    public string? Apellidos { get; set; }
    public string? RazonSocial { get; set; }
    public string Correo { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public int IdCiudadResidencia { get; set; }
    public int IdPaisNacionalidad { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? Genero { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class RegisterClienteResponse
{
    public int IdCliente { get; set; }
    public int IdUsuario { get; set; }
    public string Username { get; set; } = null!;
    public string RolAsignado { get; set; } = null!;
}