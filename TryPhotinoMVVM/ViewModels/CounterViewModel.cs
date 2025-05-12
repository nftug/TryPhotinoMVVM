using System.Text.Json;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel(ViewModelMessageDispatcher dispatcher)
    : StateViewModelBase<CounterViewModelPayload, CounterActionType>(dispatcher, new(0, null, false))
{
    public override ViewModelType ViewModelType => ViewModelType.Counter;

    protected override ValueTask HandleActionAsync(CounterActionType action, JsonElement? payload)
        => action switch
        {
            CounterActionType.Increment => ChangeCountAsync(State.Value.Count + 1),
            CounterActionType.Decrement => ChangeCountAsync(State.Value.Count - 1),
            _ => ValueTask.CompletedTask,
        };

    private async ValueTask ChangeCountAsync(int value)
    {
        if (State.Value.IsProcessing) return;

        State.Value = State.Value with { Count = value, IsProcessing = true };
        await Task.Delay(500);
        State.Value = State.Value with { TwiceCount = value * 2, IsProcessing = false };
    }
}

public record CounterViewModelPayload(int Count, int? TwiceCount, bool IsProcessing);

public enum CounterActionType
{
    Increment,
    Decrement
}
