using Newtonsoft.Json;

namespace TuyaSharp.Models.Requests;

public class Response<TBody>
{
    [JsonProperty("result")]
    public TBody? Result { get; set; }

    [JsonProperty("success")]
    public bool? Success { get; set; }

    [JsonProperty("t")]
    public long? Timestamp { get; set; }

    [JsonProperty("tid")]
    public string? Tid { get; set; }

    [JsonProperty("code")]
    public int? Code { get; set; }

    [JsonProperty("msg")]
    public string? Message { get; set; }
}
