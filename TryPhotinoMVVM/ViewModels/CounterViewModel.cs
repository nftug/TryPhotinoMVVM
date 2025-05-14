using System.Reactive.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel : StateViewModelBase<CounterViewModelPayload, CounterActionType>
{
    public override ViewModelType ViewModelType => ViewModelType.Counter;

    protected override JsonTypeInfo<ViewModelMessage<CounterViewModelPayload>> StateJsonTypeInfo
        => JsonContext.Default.ViewModelMessageCounterViewModelPayload;

    public CounterViewModel(ViewModelMessageDispatcher dispatcher)
        : base(dispatcher, new(0, null, false))
    {
        State
            .DistinctUntilChanged(x => x.Count)
            .Select(x => CounterFizzBuzzEvent.Create(x.Count))
            .Where(e => e != null)
            .Subscribe(e =>
                dispatcher.DispatchEvent(ViewModelType, e, JsonContext.Default.EventMessageFizzBuzz));
    }

    protected override ValueTask HandleActionAsync(CounterActionType action, JsonElement? payload)
        => (action, payload) switch
        {
            (CounterActionType.Set, { }) => SetCountAsync(payload.Value),
            (CounterActionType.Increment, _) => ChangeCountAsync(State.Value.Count + 1),
            (CounterActionType.Decrement, _) => ChangeCountAsync(State.Value.Count - 1),
            _ => ValueTask.CompletedTask,
        };

    private ValueTask SetCountAsync(JsonElement payload)
    {
        var actionPayload = payload.ParsePayload(JsonContext.Default.CounterSetActionPayload);
        if (actionPayload is not { }) return ValueTask.CompletedTask;
        return ChangeCountAsync(actionPayload.Value);
    }

    private async ValueTask ChangeCountAsync(long value)
    {
        if (State.Value.IsProcessing || State.Value.Count == value) return;
        if (value < 0)
        {
            State.ForceNotify();
            return;
        }

        State.Value = State.Value with { Count = value, IsProcessing = true };
        await Task.Delay(500);
        State.Value = State.Value with { TwiceCount = value * 2, IsProcessing = false };
    }
}

public record CounterViewModelPayload(long Count, long? TwiceCount, bool IsProcessing)
{
    public bool CanDecrement => Count > 0;
}

public record CounterSetActionPayload(long Value);

public record CounterFizzBuzzEvent(FizzBuzz Payload)
    : EventMessagePayload<FizzBuzz>("fizzBuzz", Payload)
{
    public static CounterFizzBuzzEvent? Create(long count)
    {
        FizzBuzz? payload = count switch
        {
            0 => null,
            var x when x % 15 == 0 => FizzBuzz.FizzBuzz,
            var x when x % 5 == 0 => FizzBuzz.Buzz,
            var x when x % 3 == 0 => FizzBuzz.Fizz,
            _ => null,
        };
        return payload is { } ? new(payload.Value) : null;
    }
}

public enum CounterActionType
{
    Set,
    Increment,
    Decrement
}

[JsonConverter(typeof(JsonStringEnumConverter<FizzBuzz>))]
public enum FizzBuzz
{
    Fizz,
    Buzz,
    FizzBuzz,
}