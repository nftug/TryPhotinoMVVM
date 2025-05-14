using System.Reactive.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel : StateViewModelBase<CounterState, CounterActionType>
{
    public override ViewModelType ViewModelType => ViewModelType.Counter;

    protected override JsonTypeInfo<StateMessage<CounterState>> StateJsonTypeInfo
        => CounterJsonContext.Default.StateMessageCounterState;

    public CounterViewModel(ViewModelMessageDispatcher dispatcher)
        : base(dispatcher, new(0, null, false))
    {
        State
            .DistinctUntilChanged(x => x.Count)
            .Select(x => CounterFizzBuzzEvent.Create(x.Count))
            .Where(e => e != null)
            .Subscribe(e => DispatchEvent(e!, CounterJsonContext.Default.EventMessageFizzBuzz));
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
        var actionPayload = payload.ParsePayload(CounterJsonContext.Default.CounterSetActionPayload);
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
