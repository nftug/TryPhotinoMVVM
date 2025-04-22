using System.Text.Json.Serialization;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.ViewModels;

namespace TryPhotinoMVVM.Constants;

[JsonSerializable(typeof(IncomingMessage))]
[JsonSerializable(typeof(OutgoingMessage<CounterOutgoingPayload>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext { }
