using System.Text.Json.Serialization;
using TryPhotinoMVVM.Dtos.Abstractions.Commands;
using TryPhotinoMVVM.Dtos.Abstractions.Events;
using TryPhotinoMVVM.Dtos.Application.Commands;
using TryPhotinoMVVM.Dtos.Application.Events;
using TryPhotinoMVVM.Dtos.Counter.Commands;
using TryPhotinoMVVM.Dtos.Counter.Events;

namespace TryPhotinoMVVM.Constants;

#region Message
[JsonSerializable(typeof(EventMessage<DummyEventPayload>))]
[JsonSerializable(typeof(EventMessage<ViewModelErrorEvent.ViewModelError>))]
[JsonSerializable(typeof(CommandMessage))]
[JsonSerializable(typeof(InitCommandPayload))]
#endregion

#region
[JsonSerializable(typeof(EventMessage<MessageBoxResultEvent.MessageBoxResultType>))]
[JsonSerializable(typeof(MessageBoxCommandPayload))]
#endregion

#region Counter
[JsonSerializable(typeof(EventMessage<CounterStateDto>))]
[JsonSerializable(typeof(EventMessage<CounterFizzBuzzDto>))]
[JsonSerializable(typeof(CounterSetCommandPayload))]
#endregion

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
public partial class JsonContext : JsonSerializerContext;
