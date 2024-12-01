using TuyaSharp.Enums;

namespace TuyaSharp;

public class TuyaSharpClientOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required RegionHost RegionHost { get; set; }
    public Languages Language { get; set; } = Languages.English;
}
