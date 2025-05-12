using System.Text.Json.Serialization;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.ViewModels;

namespace TryPhotinoMVVM.Constants;

[JsonSerializable(typeof(CommandMessage))]
[JsonSerializable(typeof(ViewModelMessage<CounterViewModelPayload>))]
[JsonSerializable(typeof(CounterSetActionPayload))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
