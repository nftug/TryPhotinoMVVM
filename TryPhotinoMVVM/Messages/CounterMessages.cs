using System.Text.Json.Serialization;
using TryPhotinoMVVM.Domain.Enums;
using TryPhotinoMVVM.Dtos;

namespace TryPhotinoMVVM.Messages;

#region State
public record CounterStateEvent(CounterStateDto Payload)
    : EventPayload<CounterStateDto>("state", Payload);

#endregion

#region Events
public record CounterFizzBuzzEvent(FizzBuzz Payload)
    : EventPayload<FizzBuzz>("fizzBuzz", Payload);
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
