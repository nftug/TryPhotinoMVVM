using System.Text.Json.Serialization;
using TryPhotinoMVVM.Domain.Enums;
using TryPhotinoMVVM.Dtos;

namespace TryPhotinoMVVM.Messages;

#region State
public record CounterStateEvent(CounterStateDto Payload)
    : EventMessage<CounterStateDto>("state", Payload);

#endregion

#region Events
public record CounterFizzBuzzEvent(CounterFizzBuzzDto Payload)
    : EventMessage<CounterFizzBuzzDto>("fizzBuzz", Payload);
#endregion

#region Actions
public enum CounterCommandType
{
    Set,
    Increment,
    Decrement
}

public record CounterSetCommandPayload(long Value);
#endregion
