using System;
using TuyaSharp.Abstractions;
using TuyaSharp.Enums;

namespace TuyaSharp.Services;

public class LanguageResolver : ILanguageResolver
{
    public string Resolve(Languages language) =>
        language switch
        {
            Languages.English => "en",
            Languages.Chinese => "zh",
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, "Invalid language.")
        };
}
