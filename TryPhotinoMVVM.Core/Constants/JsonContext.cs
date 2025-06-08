using System.Text.Json.Serialization;
using BrowserBridge;
using TryPhotinoMVVM.Core.Dtos.Counter.Commands;
using TryPhotinoMVVM.Core.Dtos.Counter.Events;

namespace TryPhotinoMVVM.Core.Constants;

#region Counter
[JsonSerializable(typeof(EventMessage<CounterStateDto>))]
[JsonSerializable(typeof(EventMessage<CounterFizzBuzzDto>))]
[JsonSerializable(typeof(CounterSetCommandPayload))]
#endregion

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class JsonContext : JsonSerializerContext;
