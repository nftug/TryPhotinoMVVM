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

    public static ValueTask HandlePayloadAsync<T>(
        this JsonElement payload, JsonTypeInfo<T> jsonTypeInfo, Func<T, ValueTask> callback)
    {
        var parsed = payload.ParsePayload(jsonTypeInfo);
        return parsed != null ? callback(parsed) : ValueTask.CompletedTask;
    }

    public static ValueTask HandlePayloadSync<T>(
        this JsonElement payload, JsonTypeInfo<T> jsonTypeInfo, Action<T> callback)
    {
        var parsed = payload.ParsePayload(jsonTypeInfo);
        if (parsed == null) return ValueTask.CompletedTask;
        callback(parsed);
        return ValueTask.CompletedTask;
    }
}
