using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

// ============================================================
// DTOs que mapean exactamente los contratos REST de MS ReservasF.
// Validados contra los endpoints y modelos reales del microservicio.
// Esquema BD: ventas
// ============================================================

// ------------------------------------------------------------
// GET /api/v1/reservas/{id_reserva}
// POST /api/v1/reservas
// ------------------------------------------------------------

/// <summary>
/// Representa una reserva devuelta por MS ReservasF.
/// Estados: PEN (pendiente), CON (confirmada), EMI (emitida),
///          CAN (cancelada), FIN (finalizada)
/// </summary>
public class ReservaDto
{
    [JsonPropertyName("idReserva")]
    public int IdReserva { get; set; }

    [JsonPropertyName("guidReserva")]
    public Guid GuidReserva { get; set; }

    [JsonPropertyName("codigoReserva")]
    public string CodigoReserva { get; set; } = null!;

    [JsonPropertyName("idCliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("idVuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("fechaReservaUtc")]
    public DateTime FechaReservaUtc { get; set; }

    [JsonPropertyName("fechaInicio")]
    public DateTime FechaInicio { get; set; }

    [JsonPropertyName("fechaFin")]
    public DateTime FechaFin { get; set; }

    [JsonPropertyName("subtotalReserva")]
    public decimal SubtotalReserva { get; set; }

    [JsonPropertyName("valorIva")]
    public decimal ValorIva { get; set; }

    [JsonPropertyName("totalReserva")]
    public decimal TotalReserva { get; set; }

    [JsonPropertyName("origenCanalReserva")]
    public string OrigenCanalReserva { get; set; } = null!;

    [JsonPropertyName("estadoReserva")]
    public string EstadoReserva { get; set; } = null!;

    [JsonPropertyName("fechaConfirmacionUtc")]
    public DateTime? FechaConfirmacionUtc { get; set; }

    [JsonPropertyName("fechaCancelacionUtc")]
    public DateTime? FechaCancelacionUtc { get; set; }

    [JsonPropertyName("motivoCancelacion")]
    public string? MotivoCancelacion { get; set; }

    [JsonPropertyName("contactoEmail")]
    public string? ContactoEmail { get; set; }

    [JsonPropertyName("contactoTelefono")]
    public string? ContactoTelefono { get; set; }

    [JsonPropertyName("observaciones")]
    public string? Observaciones { get; set; }

    [JsonPropertyName("esEliminado")]
    public bool EsEliminado { get; set; }

    [JsonPropertyName("detalles")]
    public List<ReservaDetalleDto> Detalles { get; set; } = [];
}

/// <summary>
/// Detalle de reserva — una línea por pasajero/asiento.
/// Relación real: ReservaDetalle 1:0..1 Boleto
/// </summary>
public class ReservaDetalleDto
{
    [JsonPropertyName("idDetalle")]
    public int IdDetalle { get; set; }

    [JsonPropertyName("idReserva")]
    public int IdReserva { get; set; }

    [JsonPropertyName("idPasajero")]
    public int IdPasajero { get; set; }

    [JsonPropertyName("idAsiento")]
    public int IdAsiento { get; set; }

    [JsonPropertyName("subtotalLinea")]
    public decimal SubtotalLinea { get; set; }

    [JsonPropertyName("valorIvaLinea")]
    public decimal ValorIvaLinea { get; set; }

    [JsonPropertyName("totalLinea")]
    public decimal TotalLinea { get; set; }

    [JsonPropertyName("estadoDetalle")]
    public string EstadoDetalle { get; set; } = null!;

    [JsonPropertyName("esEliminado")]
    public bool EsEliminado { get; set; }
}

// ------------------------------------------------------------
// POST /api/v1/reservas — Request de creación
// ------------------------------------------------------------

/// <summary>
/// Request que el Bus envía a MS ReservasF para crear una reserva.
/// Para rol CLIENTE, id_cliente viene del JWT — no del body.
/// </summary>
public class CrearReservaRequestDto
{
    [JsonPropertyName("id_cliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("id_vuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [JsonPropertyName("fecha_fin")]
    public DateTime FechaFin { get; set; }

    [JsonPropertyName("origen_canal_reserva")]
    public string OrigenCanalReserva { get; set; } = "BUS";

    [JsonPropertyName("contacto_email")]
    public string? ContactoEmail { get; set; }

    [JsonPropertyName("contacto_telefono")]
    public string? ContactoTelefono { get; set; }

    [JsonPropertyName("observaciones")]
    public string? Observaciones { get; set; }

    [JsonPropertyName("detalles")]
    public List<CrearReservaDetalleDto> Detalles { get; set; } = [];
}

public class CrearReservaDetalleDto
{
    [JsonPropertyName("id_pasajero")]
    public int IdPasajero { get; set; }

    [JsonPropertyName("id_asiento")]
    public int IdAsiento { get; set; }
}

// ------------------------------------------------------------
// GET /api/v1/boletos/{id_boleto}
// POST /api/v1/boletos
// ------------------------------------------------------------

/// <summary>
/// Representa un boleto devuelto por MS ReservasF.
/// </summary>
public class BoletoDto
{
    [JsonPropertyName("idBoleto")]
    public int IdBoleto { get; set; }

    [JsonPropertyName("idReserva")]
    public int IdReserva { get; set; }

    [JsonPropertyName("idDetalle")]
    public int IdDetalle { get; set; }

    [JsonPropertyName("idVuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("idAsiento")]
    public int IdAsiento { get; set; }

    [JsonPropertyName("idFactura")]
    public int IdFactura { get; set; }

    [JsonPropertyName("codigoBoleto")]
    public string CodigoBoleto { get; set; } = null!;

    [JsonPropertyName("clase")]
    public string Clase { get; set; } = null!;

    [JsonPropertyName("precioVueloBase")]
    public decimal PrecioVueloBase { get; set; }

    [JsonPropertyName("precioAsientoExtra")]
    public decimal PrecioAsientoExtra { get; set; }

    [JsonPropertyName("impuestosBoleto")]
    public decimal ImpuestosBoleto { get; set; }

    [JsonPropertyName("cargoEquipaje")]
    public decimal CargoEquipaje { get; set; }

    [JsonPropertyName("precioFinal")]
    public decimal PrecioFinal { get; set; }

    [JsonPropertyName("estadoBoleto")]
    public string EstadoBoleto { get; set; } = null!;

    [JsonPropertyName("fechaEmision")]
    public DateTime FechaEmision { get; set; }

    [JsonPropertyName("esEliminado")]
    public bool EsEliminado { get; set; }
}

// ------------------------------------------------------------
// GET /api/v1/facturas/{id_factura}
// POST /api/v1/facturas
// ------------------------------------------------------------

/// <summary>
/// Representa una factura devuelta por MS ReservasF.
/// Una reserva tiene exactamente una factura (índice único en BD).
/// Estados: ABI (abierta), APR (aprobada), INA (inactiva)
/// </summary>
public class FacturaDto
{
    [JsonPropertyName("idFactura")]
    public int IdFactura { get; set; }

    [JsonPropertyName("guidFactura")]
    public Guid GuidFactura { get; set; }

    [JsonPropertyName("idCliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("idReserva")]
    public int IdReserva { get; set; }

    [JsonPropertyName("numeroFactura")]
    public string NumeroFactura { get; set; } = null!;

    [JsonPropertyName("fechaEmision")]
    public DateTime FechaEmision { get; set; }

    [JsonPropertyName("subtotal")]
    public decimal Subtotal { get; set; }

    [JsonPropertyName("valorIva")]
    public decimal ValorIva { get; set; }

    [JsonPropertyName("cargoServicio")]
    public decimal CargoServicio { get; set; }

    [JsonPropertyName("total")]
    public decimal Total { get; set; }

    [JsonPropertyName("estadoFactura")]
    public string EstadoFactura { get; set; } = null!;

    [JsonPropertyName("observacionesFactura")]
    public string? ObservacionesFactura { get; set; }

    [JsonPropertyName("esEliminado")]
    public bool EsEliminado { get; set; }
}

// ------------------------------------------------------------
// GET /api/v1/boletos/{id_boleto}/equipaje
// POST /api/v1/boletos/{id_boleto}/equipaje
// ------------------------------------------------------------

/// <summary>
/// Representa un equipaje devuelto por MS ReservasF.
/// Precio fijo: $45 por maleta de bodega según reglas de negocio.
/// </summary>
public class EquipajeDto
{
    [JsonPropertyName("idEquipaje")]
    public int IdEquipaje { get; set; }

    [JsonPropertyName("idBoleto")]
    public int IdBoleto { get; set; }

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = null!;

    [JsonPropertyName("pesoKg")]
    public decimal PesoKg { get; set; }

    [JsonPropertyName("descripcionEquipaje")]
    public string? DescripcionEquipaje { get; set; }

    [JsonPropertyName("precioExtra")]
    public decimal PrecioExtra { get; set; }

    [JsonPropertyName("dimensionesCm")]
    public string? DimensionesCm { get; set; }

    [JsonPropertyName("numeroEtiqueta")]
    public string NumeroEtiqueta { get; set; } = null!;

    [JsonPropertyName("estadoEquipaje")]
    public string EstadoEquipaje { get; set; } = null!;

    [JsonPropertyName("esEliminado")]
    public bool EsEliminado { get; set; }
}

/// <summary>
/// Request para agregar equipaje a un boleto.
/// </summary>
public class AgregarEquipajeRequestDto
{
    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = null!;

    [JsonPropertyName("pesoKg")]
    public decimal PesoKg { get; set; }

    [JsonPropertyName("descripcionEquipaje")]
    public string? DescripcionEquipaje { get; set; }

    [JsonPropertyName("dimensionesCm")]
    public string? DimensionesCm { get; set; }
}

/// <summary>
/// Wrapper genérico de respuesta de MS ReservasF.
/// </summary>
public class ReservasApiResponseDto<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = [];
}