using Middleware.Vuelos.Api.Extensions;
using Middleware.Vuelos.Api.Handlers;
using Microservicio.ReservasF.Grpc;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddBusApiVersioning();
builder.Services.AddBusCors(builder.Configuration);
builder.Services.AddBusSwagger();

// HttpClients
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenForwardingHandler>();
builder.Services.AddSeguridadHttpClient(builder.Configuration);
builder.Services.AddGeografiaHttpClient(builder.Configuration);
builder.Services.AddAeropuertosHttpClient(builder.Configuration);
builder.Services.AddClientesHttpClient(builder.Configuration);
builder.Services.AddVuelosHttpClient(builder.Configuration);
builder.Services.AddReservasFHttpClient(builder.Configuration);

// gRPC Client — MS ReservasF
builder.Services.AddGrpcClient<ReservasGrpc.ReservasGrpcClient>(options =>
{
    options.Address = new Uri(
        builder.Configuration["Microservicios:ReservasFGrpcUrl"]!);
});

// JWT
builder.Services.AddBusJwtAuthentication(builder.Configuration);

// Orchestrators
builder.Services.AddBusOrchestrators();

var app = builder.Build();

app.UseBusSwagger();
app.UseCors("BusPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();