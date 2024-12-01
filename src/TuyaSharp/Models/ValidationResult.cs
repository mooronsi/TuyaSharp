namespace TuyaSharp.Models;

internal class ValidationResult
{
    internal bool IsValid { get; set; }
    internal string[] Errors { get; set; } = [];

    internal static ValidationResult Ok => new()
        { IsValid = true };

    internal static ValidationResult Fail(params string[] errors) =>
        new() { IsValid = false, Errors = errors };
}
