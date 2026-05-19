using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Services;
using Middleware.Vuelos.Api.Handlers;
namespace Middleware.Vuelos.Api.Extensions;

/// <summary>
/// Extension de registro del HttpClient de MS Aeropuertos en el Bus.
///
/// Agregar en Program.cs:
///   builder.Services.AddAeropuertosHttpClient(builder.Configuration);
///
/// La URL base se lee de appsettings.json:
///   Microservicios:AeropuertosBaseUrl
///
/// Ejemplo appsettings.json:
/// {
///   "Microservicios": {
///     "SeguridadBaseUrl":  "https://localhost:44375",
///     "GeografiaBaseUrl":  "https://localhost:44395",
///     "AeropuertosBaseUrl":"https://localhost:44363"
///   }
/// }
/// </summary>
public static class HttpClientAeropuertosExtensions
{
    public static IServiceCollection AddAeropuertosHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseUrl = configuration["Microservicios:AeropuertosBaseUrl"]
            ?? throw new InvalidOperationException(
                "La configuración 'Microservicios:AeropuertosBaseUrl' es requerida. " +
                "Verifique appsettings.json o variables de entorno.");

        services.AddHttpClient<IAeropuertosClient, AeropuertosClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        services.AddHttpClient<AeropuertosClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        services.AddScoped<IAeropuertosDataService, AeropuertosDataService>();

        return services;
    }
}