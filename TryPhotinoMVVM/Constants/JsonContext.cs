using System.Text.Json.Serialization;
using BrowserBridge;
using TryPhotinoMVVM.Dtos.Counter.Commands;
using TryPhotinoMVVM.Dtos.Counter.Events;

namespace TryPhotinoMVVM.Constants;

#region Counter
[JsonSerializable(typeof(EventMessage<CounterStateDto>))]
[JsonSerializable(typeof(EventMessage<CounterFizzBuzzDto>))]
[JsonSerializable(typeof(CounterSetCommandPayload))]
#endregion

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
