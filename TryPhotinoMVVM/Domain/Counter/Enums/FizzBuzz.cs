using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Domain.Counter.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<FizzBuzz>))]
public enum FizzBuzz
{
    Fizz,
    Buzz,
    FizzBuzz,
}