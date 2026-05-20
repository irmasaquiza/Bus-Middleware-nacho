using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Services;
using Middleware.Vuelos.Api.Handlers;
namespace Middleware.Vuelos.Api.Extensions;

/// <summary>
/// Extension de registro del HttpClient de MS Geografía en el Bus.
///
/// Agregar en Program.cs:
///   builder.Services.AddGeografiaHttpClient(builder.Configuration);
///
/// La URL base se lee de appsettings.json:
///   Microservicios:GeografiaBaseUrl
///
/// Ejemplo appsettings.json:
/// {
///   "Microservicios": {
///     "SeguridadBaseUrl": "https://localhost:44375",
///     "GeografiaBaseUrl": "https://localhost:44395"
///   }
/// }
/// </summary>
public static class HttpClientGeografiaExtensions
{
    public static IServiceCollection AddGeografiaHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseUrl = configuration["Microservicios:GeografiaBaseUrl"]
            ?? throw new InvalidOperationException(
                "La configuración 'Microservicios:GeografiaBaseUrl' es requerida. " +
                "Verifique appsettings.json o variables de entorno.");

        // Named HttpClient "GeografiaClient"
        services.AddHttpClient<IGeografiaClient, GeografiaClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // Geografía es solo catálogo — las consultas son rápidas.
            client.Timeout = TimeSpan.FromSeconds(15);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        services.AddHttpClient<GeografiaClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(50);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        // Registrar el DataService de Geografía.
        services.AddScoped<IGeografiaDataService, GeografiaDataService>();

        return services;
    }
}