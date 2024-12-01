using System;
using System.Linq;
using System.Net.Http;

namespace TuyaSharp.Utils;

internal static class StringToSignGenerator
{
    private static readonly string[] SupportedHttpMethods = ["GET", "POST", "PUT", "DELETE"];

    internal static string Generate(
        HttpMethod httpMethod,
        string content,
        string path)
    {
        var httpMethodString = httpMethod.Method;
        if (!SupportedHttpMethods.Contains(httpMethodString))
        {
            throw new ArgumentException("Unsupported HTTP method.", nameof(httpMethod));
        }

        // ToDo: Add Optional_Signature_key support
        return $"{httpMethodString}\n{content}\n\n{path}";
    }
}
