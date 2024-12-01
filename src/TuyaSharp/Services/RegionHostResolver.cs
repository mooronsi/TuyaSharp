using System;
using TuyaSharp.Abstractions;
using TuyaSharp.Enums;

namespace TuyaSharp.Services;

internal class RegionHostResolver : IRegionHostResolver
{
    public string Resolve(RegionHost regionHost) =>
        regionHost switch
        {
            RegionHost.China => Consts.ChinaRegionHost,
            RegionHost.WesternAmerica => Consts.WesternAmericaRegionHost,
            RegionHost.EasternAmerica => Consts.EasternAmericaRegionHost,
            RegionHost.CentralEurope => Consts.CentralEuropeRegionHost,
            RegionHost.WesternEurope => Consts.WesternEuropeRegionHost,
            RegionHost.India => Consts.IndiaRegionHost,
            _ => throw new ArgumentOutOfRangeException(nameof(regionHost), regionHost, "Unknown region host.")
        };
}
