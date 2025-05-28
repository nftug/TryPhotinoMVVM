using System.Text.Json.Serialization;
using TryPhotinoMVVM.Domain.Enums;
using TryPhotinoMVVM.Dtos;

namespace TryPhotinoMVVM.Messages;

#region State
public record CounterStateEvent(CounterStateDto Payload)
    : EventMessage<CounterStateDto>(ViewModelType.Counter, "state", Payload);

#endregion

#region Events
public record CounterFizzBuzzEvent(CounterFizzBuzzDto Payload)
    : EventMessage<CounterFizzBuzzDto>(ViewModelType.Counter, "fizzBuzz", Payload);
#endregion

#region Actions
public enum CounterActionType
{
    Set,
    Increment,
    Decrement
}

public record CounterSetActionPayload(long Value);
#endregion
