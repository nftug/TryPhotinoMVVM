using System.Globalization;
using System.Reflection;

namespace TryPhotinoMVVM.Extensions;

public static class AssemblyExtensions
{
    // See: https://www.meziantou.net/getting-the-date-of-build-of-a-dotnet-assembly-at-runtime.htm
    public static DateTime GetBuildDate(this Assembly assembly)
    {
        const string BuildVersionMetadataPrefix = "+build";

        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (attribute?.InformationalVersion != null)
        {
            var value = attribute.InformationalVersion;
            var index = value.IndexOf(BuildVersionMetadataPrefix);
            if (index > 0)
            {
                value = value.Substring(index + BuildVersionMetadataPrefix.Length);
                if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                {
                    return result;
                }
            }
        }

        return default;
    }

    public static string GetBuildDateHash(this Assembly assembly)
        => assembly.GetBuildDate().ToString("yyyyMMddHHmmss");
}
