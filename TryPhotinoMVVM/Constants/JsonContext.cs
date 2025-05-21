using System.Text.Json.Serialization;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Models;

namespace TryPhotinoMVVM.Constants;

#region Message
[JsonSerializable(typeof(EventMessage<EventEmptyPayload>))]
[JsonSerializable(typeof(EventMessage<ErrorMessage>))]
[JsonSerializable(typeof(CommandMessage))]
#endregion

#region Counter
[JsonSerializable(typeof(EventMessage<CounterState>))]
[JsonSerializable(typeof(EventMessage<FizzBuzz>))]
[JsonSerializable(typeof(CounterSetActionPayload))]
#endregion

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
