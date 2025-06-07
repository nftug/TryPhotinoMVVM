using System.Text.Json;
using Microsoft.Extensions.Logging;
using R3;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Dtos.Counter.Commands;
using TryPhotinoMVVM.Dtos.Counter.Events;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.Presentation.Dispatchers;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel : ViewModelBase<CounterCommandType>
{
    private readonly CounterModel _model;

    public CounterViewModel(
        EventDispatcher dispatcher, CounterModel model, ILogger<CounterViewModel> logger) : base(dispatcher)
    {
        _model = model;

        NotifyObservable(_model.CounterState)
            .Select(s => new CounterStateEvent(s!.ToDto()))
            .Subscribe(e => Dispatch(e, JsonContext.Default.EventMessageCounterStateDto))
            .AddTo(Disposable);

        NotifyObservable(_model.FizzBuzzReceivedCommand)
            .Select(v => new CounterFizzBuzzEvent(new(v)))
            .Subscribe(e =>
            {
                Dispatch(e, JsonContext.Default.EventMessageCounterFizzBuzzDto);
                logger.LogInformation("Invoked FizzBuzz in {ViewId}: {FizzBuzz}", ViewId.CurrentValue, e.Payload!.Result);
            })
            .AddTo(Disposable);
    }

    protected override ValueTask HandleActionAsync(CounterCommandType action, JsonElement? payload, Guid? commandId)
        => (action, payload) switch
        {
            (CounterCommandType.Set, { } p) => p.HandlePayloadAsync(
                JsonContext.Default.CounterSetCommandPayload, SetCountAsync),
            (CounterCommandType.Increment, _) => SetCountAsync(new(_model.CounterState.CurrentValue!.Count + 1)),
            (CounterCommandType.Decrement, _) => SetCountAsync(new(_model.CounterState.CurrentValue!.Count - 1)),
            _ => ValueTask.CompletedTask,
        };

    private async ValueTask SetCountAsync(CounterSetCommandPayload payload)
    {
        await _model.ChangeCountAsync(payload.Value);
    }
}
