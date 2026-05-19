namespace Middleware.Vuelos.DataManagement.Models.Auth;

/// <summary>
/// Modelo interno del Bus que representa el resultado de autenticación
/// contra MS Seguridad.
///
/// Este modelo viaja entre DataManagement y Business del Bus.
/// Es la versión "limpia" del LoginResponseDto de la capa DataAccess.
/// </summary>
public class TokenDataModel
{
    /// <summary>JWT emitido por MS Seguridad. Firmado con HMAC SHA-256.</summary>
    public string Token { get; set; } = null!;

    /// <summary>Username del usuario autenticado.</summary>
    public string Usuario { get; set; } = null!;

    /// <summary>
    /// Fecha UTC de expiración del token.
    /// Configurado a 60 minutos desde la emisión (ExpirationMinutes en MS Seguridad).
    /// </summary>
    public DateTime Expiracion { get; set; }

    /// <summary>
    /// Roles activos del usuario.
    /// Valores conocidos: ADMINISTRADOR, CLIENTE, AEROLINEA, BOOKING.
    /// </summary>
    public List<string> Roles { get; set; } = [];

    /// <summary>
    /// Indica si el token sigue vigente al momento de consultar.
    /// Calculado localmente en el Bus; no requiere llamada a Seguridad.
    /// </summary>
    public bool EsVigente => DateTime.UtcNow < Expiracion;
}

/// <summary>
/// Modelo interno del Bus que representa el resultado de la creación
/// de un usuario en MS Seguridad vinculado a un cliente real.
/// </summary>
public class UsuarioClienteCreado
{
    /// <summary>Id interno del usuario en seg.usuario_app.</summary>
    public int IdUsuario { get; set; }

    /// <summary>GUID estable del usuario. Preferir para referencias en eventos.</summary>
    public Guid UsuarioGuid { get; set; }

    /// <summary>
    /// Id del cliente en MS Clientes vinculado en seg.usuario_app.id_cliente.
    /// Confirma que la referencia lógica quedó guardada correctamente.
    /// </summary>
    public int IdCliente { get; set; }

    /// <summary>Username creado.</summary>
    public string Username { get; set; } = null!;

    /// <summary>Rol asignado. Siempre CLIENTE para este flujo.</summary>
    public string RolAsignado { get; set; } = null!;

    /// <summary>CorrelationId de la saga que originó la creación.</summary>
    public string? CorrelationId { get; set; }
}