using System;

namespace TuyaSharp.Utils;

internal static class DateTimeOffsetExtensions
{
    internal static long ToTimestamp(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToUnixTimeMilliseconds();

    internal static string ToTimestampString(this DateTimeOffset dateTimeOffset) =>
        dateTimeOffset.ToUnixTimeMilliseconds().ToString();

    internal static DateTimeOffset FromTimestamp(this long timestamp) =>
        DateTimeOffset.FromUnixTimeMilliseconds(timestamp);

    internal static DateTimeOffset FromTimestampString(this string timestamp) =>
        DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timestamp));
}
