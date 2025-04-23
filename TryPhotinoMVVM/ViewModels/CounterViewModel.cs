using System.Reactive.Linq;
using System.Text.Json;
using Photino.NET;
using Reactive.Bindings;
using TryPhotinoMVVM.Message;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel : IMessageHandler
{
    public ReactivePropertySlim<int> Count { get; } = new();
    public ReactivePropertySlim<int?> TwiceCount { get; } = new();
    public ReactivePropertySlim<bool> IsProcessing { get; } = new();

    public CounterViewModel(ViewModelMessageDispatcher dispatcher)
    {
        Observable.CombineLatest(
            Count, TwiceCount, IsProcessing,
            (count, twiceCount, isProcessing) => new CounterViewModelPayload(count, twiceCount, isProcessing))
            .Subscribe(v => dispatcher.Dispatch(ViewModelType.Counter, v));
    }

    public bool CanHandle(ViewModelType type) => type == ViewModelType.Counter;

    public async void Handle(CommandPayload? payload)
    {
        if (payload?.Type == null) return;
        if (!Enum.TryParse<CounterActionType>(payload.Type, true, out var action)) return;

        switch (action)
        {
            case CounterActionType.Init:
                Count.ForceNotify();
                break;
            case CounterActionType.Increment:
                await ChangeCountAsync(Count.Value + 1);
                break;
            case CounterActionType.Decrement:
                await ChangeCountAsync(Count.Value - 1);
                break;
        }
    }

    private async ValueTask ChangeCountAsync(int value)
    {
        if (IsProcessing.Value) return;

        Count.Value = value;

        IsProcessing.Value = true;
        await Task.Delay(500);
        TwiceCount.Value = Count.Value * 2;
        IsProcessing.Value = false;
    }
}

public record CounterViewModelPayload(int Count, int? TwiceCount, bool IsProcessing);

public enum CounterActionType
{
    Init,
    Increment,
    Decrement
}
