using System.Text.Json.Serialization;
using TryPhotinoMVVM.Domain.Counter;
using TryPhotinoMVVM.Domain.Enums;
using TryPhotinoMVVM.Dtos;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.Constants;

#region Message
[JsonSerializable(typeof(EventMessage<EventEmptyPayload>))]
[JsonSerializable(typeof(EventMessage<ErrorMessage>))]
[JsonSerializable(typeof(CommandMessage))]
#endregion

#region Counter
[JsonSerializable(typeof(EventMessage<CounterStateDto>))]
[JsonSerializable(typeof(EventMessage<FizzBuzz>))]
[JsonSerializable(typeof(CounterSetActionPayload))]
#endregion

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
