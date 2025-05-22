using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<FizzBuzz>))]
public enum FizzBuzz
{
    Fizz,
    Buzz,
    FizzBuzz,
}