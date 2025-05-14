using System.Text.Json.Serialization;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.ViewModels;

namespace TryPhotinoMVVM.Constants;

[JsonSerializable(typeof(EventMessage<ErrorMessage>))]
[JsonSerializable(typeof(CommandMessage))]
[JsonSerializable(typeof(ViewModelMessage<CounterViewModelPayload>))]
[JsonSerializable(typeof(CounterSetActionPayload))]
[JsonSerializable(typeof(EventMessage<FizzBuzz>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
