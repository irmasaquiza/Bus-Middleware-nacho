using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Services;
using Middleware.Vuelos.Api.Handlers;
namespace Middleware.Vuelos.Api.Extensions;

/// <summary>
/// Extension de registro del HttpClient de MS Clientes en el Bus.
///
/// Agregar en Program.cs:
///   builder.Services.AddClientesHttpClient(builder.Configuration);
///
/// La URL base se lee de appsettings.json:
///   Microservicios:ClientesBaseUrl
///
/// Ejemplo appsettings.json:
/// {
///   "Microservicios": {
///     "SeguridadBaseUrl":   "https://localhost:44375",
///     "GeografiaBaseUrl":   "https://localhost:44395",
///     "AeropuertosBaseUrl": "https://localhost:44363",
///     "ClientesBaseUrl":    "https://localhost:44391"
///   }
/// }
/// </summary>
public static class HttpClientClientesExtensions
{
    public static IServiceCollection AddClientesHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseUrl = configuration["Microservicios:ClientesBaseUrl"]
            ?? throw new InvalidOperationException(
                "La configuración 'Microservicios:ClientesBaseUrl' es requerida. " +
                "Verifique appsettings.json o variables de entorno.");

        services.AddHttpClient<IClientesClient, ClientesClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        services.AddHttpClient<ClientesClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        services.AddScoped<IClientesDataService, ClientesDataService>();

        return services;
    }
}