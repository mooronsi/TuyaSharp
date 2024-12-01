using TuyaSharp.Enums;

namespace TuyaSharp.Abstractions;

internal interface ILanguageResolver
{
    string Resolve(Languages language);
}
