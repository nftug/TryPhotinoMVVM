using System.Text.Json;
using BrowserBridge;
using BrowserBridge.Photino;
using Microsoft.Extensions.Logging;
using R3;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Dtos.Counter.Commands;
using TryPhotinoMVVM.Dtos.Counter.Events;
using TryPhotinoMVVM.Models;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel : ViewModelBase<CounterCommandType>
{
    private readonly CounterModel _model;
    private readonly ILogger<CounterViewModel> _logger;

    public CounterViewModel(
        IEventDispatcher dispatcher, CounterModel model, ILogger<CounterViewModel> logger) : base(dispatcher)
    {
        _model = model;
        _logger = logger;
    }

    protected override void OnFirstRender()
    {
        _model.CounterState
             .Select(s => new CounterStateEvent(s!.ToDto()))
             .Subscribe(e => Dispatch(e, JsonContext.Default.EventMessageCounterStateDto))
             .AddTo(Disposable);

        _model.FizzBuzzReceivedCommand
            .Select(v => new CounterFizzBuzzEvent(new(v)))
            .Subscribe(e =>
            {
                Dispatch(e, JsonContext.Default.EventMessageCounterFizzBuzzDto);
                _logger.LogInformation("Invoked FizzBuzz in {ViewId}: {FizzBuzz}", ViewId.CurrentValue, e.Payload!.Result);
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

public class CounterViewModelResolver(AppContainerInstance container) : IViewModelResolver
{
    public string Type => "Counter";

    public IOwnedViewModel Resolve()
    {
        var owned = container.Resolve<CounterViewModel>();
        return new StrongInjectOwnedViewModel(owned);
    }
}
