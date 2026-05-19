namespace Middleware.Vuelos.Business.DTOs.Clientes;

// ── Clientes Admin ────────────────────────────────────────────────────────────

public class ClientesFiltroRequest
{
    public string? TipoIdentificacion { get; set; }
    public string? NumeroIdentificacion { get; set; }
    public string? Nombres { get; set; }
    public string? Correo { get; set; }
    public string? Estado { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ClienteResponse
{
    public int IdCliente { get; set; }
    public Guid ClienteGuid { get; set; }
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
    public string Estado { get; set; } = null!;
    public bool EsEliminado { get; set; }
}

/// <summary>
/// Request para crear cliente desde el admin.
/// El Bus orquesta: crea cliente en MS Clientes
/// y luego crea usuario en MS Seguridad.
/// </summary>
public class CrearClienteAdminRequest
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

    // Credenciales para MS Seguridad
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class CrearClienteAdminResponse
{
    public int IdCliente { get; set; }
    public string Nombres { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public int IdUsuario { get; set; }
    public string Username { get; set; } = null!;
    public string RolAsignado { get; set; } = null!;
}

public class ActualizarClienteRequest
{
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
}

// ── Pasajeros Admin ───────────────────────────────────────────────────────────

public class PasajerosFiltroRequest
{
    public int? IdCliente { get; set; }
    public string? NombrePasajero { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? Estado { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class PasajeroResponse
{
    public int IdPasajero { get; set; }
    public int? IdCliente { get; set; }
    public string NombrePasajero { get; set; } = null!;
    public string ApellidoPasajero { get; set; } = null!;
    public string TipoDocumentoPasajero { get; set; } = null!;
    public string NumeroDocumentoPasajero { get; set; } = null!;
    public DateTime? FechaNacimientoPasajero { get; set; }
    public int? IdPaisNacionalidad { get; set; }
    public string? EmailContactoPasajero { get; set; }
    public string? TelefonoContactoPasajero { get; set; }
    public string? GeneroPasajero { get; set; }
    public bool RequiereAsistencia { get; set; }
    public string? ObservacionesPasajero { get; set; }
    public string Estado { get; set; } = null!;
}

public class CrearPasajeroRequest
{
    public int? IdCliente { get; set; }
    public string NombrePasajero { get; set; } = null!;
    public string ApellidoPasajero { get; set; } = null!;
    public string TipoDocumentoPasajero { get; set; } = null!;
    public string NumeroDocumentoPasajero { get; set; } = null!;
    public DateTime? FechaNacimientoPasajero { get; set; }
    public int? IdPaisNacionalidad { get; set; }
    public string? EmailContactoPasajero { get; set; }
    public string? TelefonoContactoPasajero { get; set; }
    public string? GeneroPasajero { get; set; }
    public bool RequiereAsistencia { get; set; }
    public string? ObservacionesPasajero { get; set; }
}

public class ActualizarPasajeroRequest
{
    public string NombrePasajero { get; set; } = null!;
    public string ApellidoPasajero { get; set; } = null!;
    public string TipoDocumentoPasajero { get; set; } = null!;
    public string NumeroDocumentoPasajero { get; set; } = null!;
    public DateTime? FechaNacimientoPasajero { get; set; }
    public int? IdPaisNacionalidad { get; set; }
    public string? EmailContactoPasajero { get; set; }
    public string? TelefonoContactoPasajero { get; set; }
    public string? GeneroPasajero { get; set; }
    public bool RequiereAsistencia { get; set; }
    public string? ObservacionesPasajero { get; set; }
}

// ── Portal Cliente ────────────────────────────────────────────────────────────

public class MiPerfilResponse
{
    public int IdCliente { get; set; }
    public string Nombres { get; set; } = null!;
    public string? Apellidos { get; set; }
    public string Correo { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public int IdCiudadResidencia { get; set; }
    public int IdPaisNacionalidad { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? Genero { get; set; }
    public string Estado { get; set; } = null!;
}