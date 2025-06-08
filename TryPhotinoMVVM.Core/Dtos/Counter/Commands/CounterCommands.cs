namespace TryPhotinoMVVM.Core.Dtos.Counter.Commands;

public enum CounterCommandType
{
    Set,
    Increment,
    Decrement
}

public record CounterSetCommandPayload(long Value);
