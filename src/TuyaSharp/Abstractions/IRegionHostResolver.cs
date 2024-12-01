using TuyaSharp.Enums;

namespace TuyaSharp.Abstractions;

internal interface IRegionHostResolver
{
    string Resolve(RegionHost regionHost);
}
