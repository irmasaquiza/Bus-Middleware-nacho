using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Services;
using Middleware.Vuelos.Api.Handlers;
namespace Middleware.Vuelos.Api.Extensions;

/// <summary>
/// Extension de registro del HttpClient de MS Seguridad en el Bus.
///
/// Agrega en Program.cs:
///   builder.Services.AddSeguridadHttpClient(builder.Configuration);
///
/// La URL base se lee de appsettings.json bajo la clave:
///   Microservicios:SeguridadBaseUrl
///
/// Ejemplo appsettings.json:
/// {
///   "Microservicios": {
///     "SeguridadBaseUrl": "http://localhost:5062"
///   }
/// }
/// </summary>
public static class HttpClientSeguridadExtensions
{
    public static IServiceCollection AddSeguridadHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Lee la URL base desde configuración.
        // En desarrollo: http://localhost:5062 (perfil HTTP de MS Seguridad).
        var baseUrl = configuration["Microservicios:SeguridadBaseUrl"]
            ?? throw new InvalidOperationException(
                "La configuración 'Microservicios:SeguridadBaseUrl' es requerida. " +
                "Verifique appsettings.json o variables de entorno.");

        // Named HttpClient "SeguridadClient" inyectado en SeguridadClient.cs.
        services.AddHttpClient<ISeguridadClient, SeguridadClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // Timeout conservador para operaciones síncronas del Bus.
            // MS Seguridad es rápido (solo valida credenciales o crea un usuario).
            client.Timeout = TimeSpan.FromSeconds(30);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        // ✅ Agregar esto — registra el concreto también
        services.AddHttpClient<SeguridadClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        // Registrar el DataService de Seguridad.
        services.AddScoped<ISeguridadDataService, SeguridadDataService>();

        return services;
    }
}