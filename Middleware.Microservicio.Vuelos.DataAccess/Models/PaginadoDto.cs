using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

public class PaginadoDto<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = [];

    [JsonPropertyName("totalRegistros")]
    public int TotalRegistros { get; set; }
}