using Middleware.Vuelos.Business.Interfaces;
using Middleware.Vuelos.Business.Orchestrators;

namespace Middleware.Vuelos.Api.Extensions;

/// <summary>
/// Registra todos los Orchestrators del Bus en el contenedor de DI.
/// Agregar en Program.cs: builder.Services.AddBusOrchestrators();
/// </summary>
public static class OrchestratorExtensions
{
    public static IServiceCollection AddBusOrchestrators(
        this IServiceCollection services)
    {
        services.AddScoped<ISeguridadOrchestrator, SeguridadOrchestrator>();
        services.AddScoped<IAeropuertosOrchestrator, AeropuertosOrchestrator>();
        services.AddScoped<IVuelosOrchestrator, VuelosOrchestrator>();
        services.AddScoped<IReservaOrchestrator, ReservaOrchestrator>();
        services.AddScoped<IFacturacionOrchestrator, FacturacionOrchestrator>();
        services.AddScoped<IPortalClienteOrchestrator, PortalClienteOrchestrator>();

        return services;
    }
}