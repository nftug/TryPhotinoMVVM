using System.Text.Json.Serialization;
using TryPhotinoMVVM.Domain.Counter;
using TryPhotinoMVVM.Domain.Enums;
using TryPhotinoMVVM.Dtos;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.Constants;

#region Message
[JsonSerializable(typeof(EventMessage<EventEmptyMessage>))]
[JsonSerializable(typeof(CommandMessage))]
[JsonSerializable(typeof(InitCommandPayload))]
#endregion

#region Counter
[JsonSerializable(typeof(EventMessage<CounterStateDto>))]
[JsonSerializable(typeof(EventMessage<CounterFizzBuzzDto>))]
[JsonSerializable(typeof(CounterSetActionPayload))]
#endregion

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
