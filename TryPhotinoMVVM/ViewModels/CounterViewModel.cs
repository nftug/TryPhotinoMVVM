using System.Text.Json;
using Photino.NET;
using Reactive.Bindings;
using TryPhotinoMVVM.Message;

namespace TryPhotinoMVVM.ViewModels;

public class CounterViewModel : IMessageHandler
{
    public ReactiveProperty<int> Count { get; } = new();

    public CounterViewModel(OutgoingMessageDispatcher dispatcher)
    {
        Count.Subscribe(value =>
        {
            dispatcher.Dispatch<CounterOutgoingPayload>(ViewModelType.Counter, new(value));
        });
    }

    public bool CanHandle(ViewModelType type) => type == ViewModelType.Counter;

    public void Handle(IncomingSubMessage? payload)
    {
        if (payload?.Type == null) return;
        if (!Enum.TryParse<CounterActionType>(payload.Type, true, out var action)) return;

        switch (action)
        {
            case CounterActionType.Init:
                Count.ForceNotify();
                break;
            case CounterActionType.Increment:
                Count.Value++;
                break;
            case CounterActionType.Decrement:
                Count.Value--;
                break;
        }
    }
}

public record CounterOutgoingPayload(int Count);

public enum CounterActionType
{
    Init,
    Increment,
    Decrement
}
