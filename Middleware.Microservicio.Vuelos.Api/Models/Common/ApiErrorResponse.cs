namespace Middleware.Vuelos.Api.Models.Common;

public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = null!;
    public List<string> Errors { get; set; } = [];
    public string? TraceId { get; set; }
}