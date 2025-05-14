using System.Reactive.Disposables;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Message;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public abstract class StateViewModelBase<TPayload, TAction> : IMessageHandler, IDisposable
    where TAction : struct, Enum
{
    private bool _disposed;
    protected CompositeDisposable Disposable { get; } = new();

    public ReactivePropertySlim<TPayload> State { get; }

    protected StateViewModelBase(ViewModelMessageDispatcher dispatcher, TPayload defaultPayload)
    {
        State = new ReactivePropertySlim<TPayload>(defaultPayload).AddTo(Disposable);
        State.Subscribe(v => dispatcher.Dispatch(ViewModelType, v, StateJsonTypeInfo)).AddTo(Disposable);
    }

    public abstract ViewModelType ViewModelType { get; }

    protected abstract JsonTypeInfo<ViewModelMessage<TPayload>> StateJsonTypeInfo { get; }

    public bool CanHandle(ViewModelType type) => type == ViewModelType;

    public ValueTask HandleAsync(CommandMessagePayload payload)
    {
        if (payload.Type.Equals(DefaultActionType.Init, StringComparison.OrdinalIgnoreCase))
            return HandleInitAsync();
        if (!Enum.TryParse<TAction>(payload.Type, true, out var action))
            return ValueTask.CompletedTask;

        return HandleActionAsync(action, payload.Payload);
    }

    protected abstract ValueTask HandleActionAsync(TAction action, JsonElement? payload);

    protected virtual ValueTask HandleInitAsync()
    {
        State.ForceNotify();
        return ValueTask.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) Disposable.Dispose();
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
