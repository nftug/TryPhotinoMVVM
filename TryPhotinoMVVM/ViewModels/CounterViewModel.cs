using System.Text.Json;
using Microsoft.Extensions.Logging;
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
    private readonly CounterModel _model;

    public CounterViewModel(
        EventDispatcher dispatcher, CounterModel model, ILogger<CounterViewModel> logger) : base(dispatcher)
    {
        _model = model.AddTo(Disposable);

        _model.CounterState
            .Where(v => v != null)
            .Select(s => new CounterStateEvent(s!.ToDto()))
            .Subscribe(e => Dispatch(e, JsonContext.Default.EventMessageCounterStateDto))
            .AddTo(Disposable);

        _model.FizzBuzzReceivedCommand
            .Select(v => new CounterFizzBuzzEvent(new(v)))
            .Subscribe(e =>
            {
                Dispatch(e, JsonContext.Default.EventMessageCounterFizzBuzzDto);
                logger.LogInformation("Invoked FizzBuzz in {ViewId}: {FizzBuzz}", ViewId, e.Payload!.Result);
            })
            .AddTo(Disposable);
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

    public override void HandleInit()
    {
        _model.ForceNotify();
    }
}
