using System.Text.Json;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.TinyLinq;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.ViewModels.Abstractions;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel : ViewModelBase<CounterActionType>
{
    public override ViewModelType ViewModelType => ViewModelType.Counter;

    private readonly CounterModel _model;

    public CounterViewModel(
        ViewModelEventDispatcher dispatcher, CounterModel model) : base(dispatcher)
    {
        _model = model;

        _model.CounterState
            .Where(v => v != null)
            .Select(s => new CounterStateEvent(s!.ToDto()))
            .Subscribe(e => Dispatch(e, JsonContext.Default.EventMessageCounterStateDto));

        _model.FizzBuzzReceivedCommand
            .Select(v => new CounterFizzBuzzEvent(v))
            .Subscribe(e => Dispatch(e, JsonContext.Default.EventMessageFizzBuzz));
    }

    protected override ValueTask HandleActionAsync(CounterActionType action, JsonElement? payload)
        => (action, payload) switch
        {
            (CounterActionType.Set, { } p) => p.HandlePayloadAsync(
                JsonContext.Default.CounterSetActionPayload, SetCountAsync),
            (CounterActionType.Increment, _) => SetCountAsync(new(_model.CounterState.Value!.Count + 1)),
            (CounterActionType.Decrement, _) => SetCountAsync(new(_model.CounterState.Value!.Count - 1)),
            _ => ValueTask.CompletedTask,
        };

    private async ValueTask SetCountAsync(CounterSetActionPayload payload)
    {
        await _model.ChangeCountAsync(payload.Value);
    }

    protected override ValueTask HandleInitAsync()
    {
        _model.ForceNotify();
        return ValueTask.CompletedTask;
    }
}
