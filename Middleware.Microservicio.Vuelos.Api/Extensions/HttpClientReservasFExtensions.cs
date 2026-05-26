using Middleware.Vuelos.DataAccess.Clients;
using Middleware.Vuelos.DataAccess.Clients.Interfaces;
using Middleware.Vuelos.DataManagement.Interfaces;
using Middleware.Vuelos.DataManagement.Services;
using Middleware.Vuelos.Api.Handlers;
namespace Middleware.Vuelos.Api.Extensions;

public static class HttpClientReservasFExtensions
{
    public static IServiceCollection AddReservasFHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseUrl = configuration["Microservicios:ReservasFBaseUrl"]
            ?? throw new InvalidOperationException(
                "La configuración 'Microservicios:ReservasFBaseUrl' es requerida. " +
                "Verifique appsettings.json o variables de entorno.");

        services.AddHttpClient<IReservasFClient, ReservasFClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(180);
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        services.AddHttpClient<ReservasFClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(180); // ← 60s igual que el original
        }).AddHttpMessageHandler<TokenForwardingHandler>(); // ✅ agregar esto


        services.AddScoped<IReservasDataService, ReservasDataService>();

        return services;
    }
}