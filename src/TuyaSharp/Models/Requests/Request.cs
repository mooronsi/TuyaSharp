using System.Collections.Generic;
using System.Net.Http;

namespace TuyaSharp.Models.Requests;

public class Request<TBody>
{
    public required HttpMethod HttpMethod { get; init; } = HttpMethod.Get;
    public required string? Path { get; init; } = default;
    public SortedDictionary<string, string>? QueryParameters { get; set; } = default;
    public TBody? Body { get; set; } = default;
}
