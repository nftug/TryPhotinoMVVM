using System.Text.Json.Serialization;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.ViewModels;

namespace TryPhotinoMVVM.Constants;

[JsonSerializable(typeof(ViewModelMessage<ErrorMessage>))]
[JsonSerializable(typeof(CommandMessage))]
[JsonSerializable(typeof(ViewModelMessage<CounterViewModelPayload>))]
[JsonSerializable(typeof(CounterSetActionPayload))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
