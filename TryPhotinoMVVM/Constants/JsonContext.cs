using System.Text.Json.Serialization;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Models;

namespace TryPhotinoMVVM.Constants;

[JsonSerializable(typeof(EventMessage<EventEmptyPayload>))]
[JsonSerializable(typeof(EventMessage<ErrorMessage>))]
[JsonSerializable(typeof(CommandMessage))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
