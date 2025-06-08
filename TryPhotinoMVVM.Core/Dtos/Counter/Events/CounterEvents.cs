using BrowserBridge;
using TryPhotinoMVVM.Core.Domain.Counter.Enums;

namespace TryPhotinoMVVM.Core.Dtos.Counter.Events;

#region Events
public record CounterStateEvent(CounterStateDto Payload)
    : EventMessage<CounterStateDto>("state", Payload);

public record CounterFizzBuzzEvent(CounterFizzBuzzDto Payload)
    : EventMessage<CounterFizzBuzzDto>("fizzBuzz", Payload);
#endregion

#region Dtos
public record CounterFizzBuzzDto(FizzBuzz Result);

public record CounterStateDto(long Count, long? TwiceCount, bool IsProcessing, bool CanDecrement);
#endregion
