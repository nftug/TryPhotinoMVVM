using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace TryPhotinoMVVM.Extensions;

public static class JsonElementExtensions
{
    public static T? ParsePayload<T>(this JsonElement element, JsonTypeInfo<T> jsonTypeInfo)
    {
        if (element.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            return default;

        return element.Deserialize<T>(jsonTypeInfo);
    }
}
