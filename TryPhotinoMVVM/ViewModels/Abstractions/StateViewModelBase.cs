using System.Reactive.Disposables;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Message;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public abstract class StateViewModelBase<TState, TAction> : IMessageHandler, IDisposable
    where TAction : struct, Enum
{
    private bool _disposed;
    protected CompositeDisposable Disposable { get; } = new();

    private readonly ViewModelMessageDispatcher _dispatcher;

    public ReactivePropertySlim<TState> State { get; }

    protected StateViewModelBase(ViewModelMessageDispatcher dispatcher, TState defaultPayload)
    {
        _dispatcher = dispatcher;
        State = new ReactivePropertySlim<TState>(defaultPayload).AddTo(Disposable);
        State.Subscribe(v => dispatcher.Dispatch(ViewModelType, v, StateJsonTypeInfo)).AddTo(Disposable);
    }

    public abstract ViewModelType ViewModelType { get; }

    protected abstract JsonTypeInfo<StateMessage<TState>> StateJsonTypeInfo { get; }

    public bool CanHandle(ViewModelType type) => type == ViewModelType;

    public ValueTask HandleAsync(CommandMessagePayload payload)
    {
        if (payload.Type.Equals(DefaultActionType.Init, StringComparison.OrdinalIgnoreCase))
            return HandleInitAsync();
        if (!Enum.TryParse<TAction>(payload.Type, true, out var action))
            return ValueTask.CompletedTask;

        return HandleActionAsync(action, payload.Payload);
    }

    protected void DispatchEvent<T>(EventMessagePayload<T> payload, JsonTypeInfo<EventMessage<T>> jsonTypeInfo)
        => _dispatcher.DispatchEvent(ViewModelType, payload, jsonTypeInfo);

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
