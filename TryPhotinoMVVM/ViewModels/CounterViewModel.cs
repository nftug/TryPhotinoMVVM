using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.TinyLinq;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel : ViewModelBase<CounterActionType>
{
    public override ViewModelType ViewModelType => ViewModelType.Counter;

    private readonly ReactivePropertySlim<CounterState> _state;

    public CounterViewModel(ViewModelEventDispatcher dispatcher) : base(dispatcher)
    {
        _state = new ReactivePropertySlim<CounterState>(new(0, null, false)).AddTo(Disposable);

        _state
            .Select(s => new CounterStateEvent(s))
            .Subscribe(e => Dispatch(e, JsonContext.Default.EventMessageCounterState))
            .AddTo(Disposable);
    }

    protected override ValueTask HandleActionAsync(CounterActionType action, JsonElement? payload)
        => (action, payload) switch
        {
            (CounterActionType.Set, { }) => SetCountAsync(payload.Value),
            (CounterActionType.Increment, _) => ChangeCountAsync(_state.Value.Count + 1),
            (CounterActionType.Decrement, _) => ChangeCountAsync(_state.Value.Count - 1),
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
        if (_state.Value.IsProcessing || _state.Value.Count == value) return;
        if (value < 0)
        {
            _state.ForceNotify();
            return;
        }

        _state.Value = _state.Value with { Count = value, IsProcessing = true };
        await Task.Delay(500);
        _state.Value = _state.Value with { TwiceCount = value * 2, IsProcessing = false };

        if (CounterFizzBuzzEvent.Create(_state.Value.Count) is { } fizzBuzzEvent)
            Dispatch(fizzBuzzEvent, JsonContext.Default.EventMessageFizzBuzz);
    }

    protected override ValueTask HandleInitAsync()
    {
        _state.ForceNotify();
        return ValueTask.CompletedTask;
    }
}
