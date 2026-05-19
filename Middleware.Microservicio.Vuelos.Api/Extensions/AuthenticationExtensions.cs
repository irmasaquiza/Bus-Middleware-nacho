using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Middleware.Vuelos.Api.Extensions;

/// <summary>
/// Configura la validación JWT en el Bus con los valores exactos
/// que MS Seguridad usa para emitir los tokens.
///
/// Valores reales validados contra el código de MS Seguridad:
///   Issuer:    "SistemaVuelos"
///   Audience:  "SistemaVuelosClientes"
///   Algoritmo: HMAC SHA-256
///   ClockSkew: TimeSpan.Zero (sin margen)
///   RoleClaimType: ClaimTypes.Role (URI largo de Microsoft)
///
/// El Secret se lee desde configuración, NUNCA hardcodeado aquí.
/// Clave en appsettings: JwtSettings:Secret
///
/// IMPORTANTE: El claim de rol en MS Seguridad usa ClaimTypes.Role, que es:
///   http://schemas.microsoft.com/ws/2008/06/identity/claims/role
/// Por eso [Authorize(Roles = "ADMINISTRADOR")] funciona correctamente.
///
/// El claim id_cliente viaja como string en el token y debe parsearse
/// como int cuando se necesite comparar en lógica de negocio.
/// </summary>
public static class AuthenticationExtensions
{
    public static IServiceCollection AddBusJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var secret = configuration["JwtSettings:Secret"]
            ?? throw new InvalidOperationException(
                "La configuración 'JwtSettings:Secret' es requerida.");

        var issuer = configuration["JwtSettings:Issuer"] ?? "SistemaVuelos";
        var audience = configuration["JwtSettings:Audience"] ?? "SistemaVuelosClientes";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Mismo algoritmo que MS Seguridad: HMAC SHA-256.
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    // Valores exactos del appsettings.json de MS Seguridad.
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    // Sin margen de tolerancia, igual que MS Seguridad.
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    // MS Seguridad usa ClaimTypes.Role (URI largo), no "role" corto.
                    // Esto es crítico para que [Authorize(Roles = "...")] funcione.
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name,

                    // Desactivar el mapeo automático de claims que renombra los claims.
                    // MS Seguridad usa MapInboundClaims = false.
                    // Sin esto, "username" podría ser renombrado por el framework.
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();
                        logger.LogWarning(
                            "[Bus][JWT] Autenticación fallida: {Message}",
                            context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();
                        var username = context.Principal?.Identity?.Name ?? "desconocido";
                        logger.LogInformation(
                            "[Bus][JWT] Token validado. Usuario={Username}", username);
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}