namespace Middleware.Vuelos.Api.Models.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];

    public static ApiResponse<T> Ok(T data, string message = "OK") => new()
    {
        Success = true,
        Message = message,
        Data = data,
        Errors = []
    };
}