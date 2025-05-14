using System.Text.Json.Serialization;
using TryPhotinoMVVM.Message;

namespace TryPhotinoMVVM.Models;

#region State
public record CounterStateEvent(CounterState Payload)
    : EventPayload<CounterState>("state", Payload);

public record CounterState(long Count, long? TwiceCount, bool IsProcessing)
{
    public bool CanDecrement => Count > 0;
}
#endregion

#region Events
public record CounterFizzBuzzEvent(FizzBuzz Payload)
    : EventPayload<FizzBuzz>("fizzBuzz", Payload)
{
    public static CounterFizzBuzzEvent? Create(long count)
    {
        FizzBuzz? payload = count switch
        {
            0 => null,
            var x when x % 15 == 0 => FizzBuzz.FizzBuzz,
            var x when x % 5 == 0 => FizzBuzz.Buzz,
            var x when x % 3 == 0 => FizzBuzz.Fizz,
            _ => null,
        };
        return payload is { } ? new(payload.Value) : null;
    }
}

[JsonConverter(typeof(JsonStringEnumConverter<FizzBuzz>))]
public enum FizzBuzz
{
    Fizz,
    Buzz,
    FizzBuzz,
}
#endregion

#region Actions
public enum CounterActionType
{
    Set,
    Increment,
    Decrement
}

public record CounterSetActionPayload(long Value);
#endregion

#region JsonContext
[JsonSerializable(typeof(EventMessage<CounterState>))]
[JsonSerializable(typeof(EventMessage<FizzBuzz>))]
[JsonSerializable(typeof(CounterSetActionPayload))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class CounterJsonContext : JsonSerializerContext;
#endregion
