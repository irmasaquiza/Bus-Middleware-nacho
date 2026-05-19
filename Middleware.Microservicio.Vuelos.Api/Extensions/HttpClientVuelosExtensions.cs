using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Services;
using Middleware.Vuelos.Api.Handlers;
namespace Middleware.Vuelos.Api.Extensions;

/// <summary>
/// Extension de registro del HttpClient de MS Vuelos en el Bus.
///
/// Agregar en Program.cs:
///   builder.Services.AddVuelosHttpClient(builder.Configuration);
///
/// La URL base se lee de appsettings.json:
///   Microservicios:VuelosBaseUrl
///
/// Ejemplo appsettings.json:
/// {
///   "Microservicios": {
///     "SeguridadBaseUrl":   "https://localhost:44375",
///     "GeografiaBaseUrl":   "https://localhost:44395",
///     "AeropuertosBaseUrl": "https://localhost:44363",
///     "ClientesBaseUrl":    "https://localhost:44391",
///     "VuelosBaseUrl":      "https://localhost:44385"
///   }
/// }
/// </summary>
public static class HttpClientVuelosExtensions
{
    public static IServiceCollection AddVuelosHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseUrl = configuration["Microservicios:VuelosBaseUrl"]
            ?? throw new InvalidOperationException(
                "La configuración 'Microservicios:VuelosBaseUrl' es requerida. " +
                "Verifique appsettings.json o variables de entorno.");

        services.AddHttpClient<IVuelosClient, VuelosClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(120);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto

        services.AddHttpClient<VuelosClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(120);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        services.AddScoped<IVuelosDataService, VuelosDataService>();

        return services;
    }
}