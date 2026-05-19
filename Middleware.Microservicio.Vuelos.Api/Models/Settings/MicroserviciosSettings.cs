namespace Middleware.Vuelos.Api.Models.Settings;

public class MicroserviciosSettings
{
    public string SeguridadBaseUrl { get; set; } = null!;
    public string GeografiaBaseUrl { get; set; } = null!;
    public string AeropuertosBaseUrl { get; set; } = null!;
    public string ClientesBaseUrl { get; set; } = null!;
    public string VuelosBaseUrl { get; set; } = null!;
    public string ReservasFBaseUrl { get; set; } = null!;
}