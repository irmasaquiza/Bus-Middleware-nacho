using System.Text.Json.Serialization;

namespace Middleware.Vuelos.DataAccess.Models;

public class VuelosPagedResultDto<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = [];

    [JsonPropertyName("totalRegistros")]
    public int TotalRegistros { get; set; }

    [JsonPropertyName("paginaActual")]
    public int PaginaActual { get; set; }

    [JsonPropertyName("totalPaginas")]
    public int TotalPaginas { get; set; }
}