namespace Middleware.Vuelos.Api.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddBusCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var origins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("BusPolicy", policy =>
            {
                policy.WithOrigins(origins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}