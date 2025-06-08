using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Core.Domain.Counter.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<FizzBuzz>))]
public enum FizzBuzz
{
    Fizz,
    Buzz,
    FizzBuzz,
}