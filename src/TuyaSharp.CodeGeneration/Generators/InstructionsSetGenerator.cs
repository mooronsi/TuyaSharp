using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TuyaSharp.CodeGeneration.Consts;
using TuyaSharp.CodeGeneration.Models;
using TuyaSharp.CodeGeneration.Templates;
using TuyaSharp.DTO;
using TuyaSharp.DTO.Device.DeviceInstructions;

namespace TuyaSharp.CodeGeneration.Generators;

public static class InstructionsSetGenerator
{
    public static void GenerateCode(
        GetDeviceInstructionsResponse response,
        string outputPath,
        string? outputNamespace = null)
    {
        var baseTemplate = InstructionsSetTemplates.Template
                           ?? throw new NullReferenceException($"{nameof(InstructionsSetTemplates.Template)} is null.");
        var booleanMethodTemplate = InstructionsSetTemplates.BooleanTemplate
                                    ?? throw new NullReferenceException(
                                        $"{nameof(InstructionsSetTemplates.BooleanTemplate)} is null.");
        var integerMethodTemplate = InstructionsSetTemplates.IntegerTemplate
                                    ?? throw new NullReferenceException(
                                        $"{nameof(InstructionsSetTemplates.IntegerTemplate)} is null.");
        var enumMethodTemplate = InstructionsSetTemplates.EnumTemplate
                                 ?? throw new NullReferenceException(
                                     $"{nameof(InstructionsSetTemplates.EnumTemplate)} is null.");

        if (response.Functions is null || response.Functions.Length == 0)
            throw new ArgumentException($"{nameof(GetDeviceInstructionsResponse.Functions)} is null or empty.");

        var methods = string.Empty;
        var enums = string.Empty;

        var category = response.Category;
        if (category is null || category.Length == 0)
            throw new ArgumentException($"{nameof(GetDeviceInstructionsResponse.Category)} is null or empty.");
        category = char.ToUpper(category[0]) + category[1..];

        foreach (var function in response.Functions)
        {
            switch (function.Type)
            {
                case InstructionsSetGeneratorConsts.BooleanType:
                    methods += GenerateBooleanMethod(booleanMethodTemplate, function);
                    break;
                case InstructionsSetGeneratorConsts.IntegerType:
                    methods += GenerateIntegerMethod(integerMethodTemplate, function);
                    break;
                case InstructionsSetGeneratorConsts.EnumType:
                    (methods, enums) = GenerateEnumMethod(enumMethodTemplate, function, category, methods, enums);
                    break;
                default:
                    throw new NotSupportedException($"Function type {function.Type} is not supported.");
            }
        }

        var code = baseTemplate
            .Replace(InstructionsSetGeneratorConsts.CategoryKey, category)
            .Replace(InstructionsSetGeneratorConsts.MethodsKey, methods);

        var enumSectionStartIndex =
            code.IndexOf(InstructionsSetGeneratorConsts.EnumsSectionStartKey, StringComparison.Ordinal);
        var enumSectionEndIndex =
            code.IndexOf(InstructionsSetGeneratorConsts.EnumsSectionEndKey, StringComparison.Ordinal);

        if (string.IsNullOrEmpty(enums))
        {
            code = code.Remove(enumSectionStartIndex,
                enumSectionEndIndex - enumSectionStartIndex + InstructionsSetGeneratorConsts.EnumsSectionEndKey.Length);
        }
        else
        {
            code = code.Replace(InstructionsSetGeneratorConsts.EnumsSectionStartKey, string.Empty)
                .Replace(InstructionsSetGeneratorConsts.EnumsSectionEndKey, string.Empty)
                .Replace(InstructionsSetGeneratorConsts.EnumsKey, enums);
        }

        outputNamespace ??= typeof(InstructionsSetGenerator).Namespace + ".InstructionsSet";
        code = code.Replace(InstructionsSetGeneratorConsts.NamespaceKey, outputNamespace);

        File.WriteAllText(outputPath, code);
    }

    private static string GenerateBooleanMethod(string template, Function function)
    {
        if (string.IsNullOrEmpty(function.Code))
            throw new ArgumentException($"{nameof(Function.Code)} is null or empty.");

        return GeneratorConsts.NewLine
               + GeneratorConsts.Tabulation
               + template
                   .Replace(InstructionsSetGeneratorConsts.ResolvedCommandNameKey, ResolveName(function.Code))
                   .Replace(InstructionsSetGeneratorConsts.CommandCodeKey, function.Code);
    }

    private static string GenerateIntegerMethod(string template, Function function)
    {
        if (string.IsNullOrEmpty(function.Code))
            throw new ArgumentException($"{nameof(Function.Code)} is null or empty.");

        if (string.IsNullOrEmpty(function.Values) || function.Values == "{}")
            throw new ArgumentException($"{nameof(Function.Values)} is null or empty.");

        var values = JsonConvert.DeserializeObject<IntegerValue>(function.Values);
        if (values?.Min == null || values.Max == null)
            throw new ArgumentException($"{nameof(Function.Values)}.Min or {nameof(Function.Values)}.Max is null.");

        return GeneratorConsts.NewLine
               + GeneratorConsts.Tabulation
               + template
                   .Replace(InstructionsSetGeneratorConsts.ResolvedCommandNameKey, ResolveName(function.Code))
                   .Replace(InstructionsSetGeneratorConsts.MinValueKey, values.Min.ToString())
                   .Replace(InstructionsSetGeneratorConsts.MaxValueKey, values.Max.ToString())
                   .Replace(InstructionsSetGeneratorConsts.CommandCodeKey, function.Code);
    }

    private static (string methods, string enums) GenerateEnumMethod(
        string template,
        Function function,
        string category,
        string methods,
        string enums)
    {
        if (string.IsNullOrEmpty(function.Code))
            throw new ArgumentException($"{nameof(Function.Code)} is null or empty.");

        if (string.IsNullOrEmpty(function.Values) || function.Values == "{}")
            throw new ArgumentException($"{nameof(Function.Values)} is null or empty.");

        var values = JsonConvert.DeserializeObject<EnumValue>(function.Values);
        if (values?.Range == null || values.Range.Length == 0)
            throw new ArgumentException($"{nameof(EnumValue.Range)} is null or empty.");

        methods += GeneratorConsts.NewLine
                   + GeneratorConsts.Tabulation
                   + template
                       .Replace(InstructionsSetGeneratorConsts.ResolvedCommandNameKey, ResolveName(function.Code))
                       .Replace(InstructionsSetGeneratorConsts.CommandCodeKey, function.Code)
                       .Replace(InstructionsSetGeneratorConsts.CategoryKey, category);

        enums += GeneratorConsts.NewLine
                 + GeneratorConsts.Tabulation
                 + $"public enum {category}{ResolveName(function.Code)}Enum"
                 + GeneratorConsts.NewLine
                 + GeneratorConsts.Tabulation
                 + "{"
                 + GeneratorConsts.NewLine;

        foreach (var value in values.Range)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{nameof(EnumValue.Range)} contains null or empty value.");

            enums += GeneratorConsts.Tabulation
                     + GeneratorConsts.Tabulation
                     + $"[{InstructionsSetGeneratorConsts.CommandValueAttributeName}(\"{value}\")]"
                     + GeneratorConsts.NewLine
                     + GeneratorConsts.Tabulation
                     + GeneratorConsts.Tabulation
                     + $"{ResolveName(function.Code)}{ResolveName(value)},"
                     + GeneratorConsts.NewLine
                     + GeneratorConsts.NewLine;
        }

        enums += GeneratorConsts.Tabulation + "}" + GeneratorConsts.NewLine;

        return (methods, enums);
    }

    private static string ResolveName(string name)
    {
        return string.Join("", name.Split('_').Select(part => char.ToUpper(part[0]) + part[1..]));
    }
}
