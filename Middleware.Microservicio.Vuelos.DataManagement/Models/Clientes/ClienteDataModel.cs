namespace Middleware.Vuelos.DataManagement.Models.Clientes;

/// <summary>
/// Modelo interno del Bus que representa un cliente de MS Clientes.
/// </summary>
public class ClienteDataModel
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

    /// <summary>Calculado localmente.</summary>
    public bool EsActivo => !EsEliminado && Estado is "ACT" or "ACTIVO";

    /// <summary>Nombre completo para logs y respuestas.</summary>
    public string NombreCompleto =>
        string.IsNullOrWhiteSpace(Apellidos)
            ? Nombres
            : $"{Nombres} {Apellidos}";
}

/// <summary>
/// Modelo interno del Bus que representa un pasajero de MS Clientes.
/// </summary>
public class PasajeroDataModel
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
    public bool EsEliminado { get; set; }

    /// <summary>Calculado localmente.</summary>
    public bool EsActivo => !EsEliminado && Estado is "ACT" or "ACTIVO";

    /// <summary>Nombre completo del pasajero.</summary>
    public string NombreCompleto => $"{NombrePasajero} {ApellidoPasajero}";
}