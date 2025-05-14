using System.Reactive.Disposables;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Message;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public abstract class ViewModelBase<TAction> : IMessageHandler, IDisposable
    where TAction : struct, Enum
{
    private bool _disposed;
    protected CompositeDisposable Disposable { get; } = new();

    private readonly ViewModelEventDispatcher _dispatcher;

    protected ViewModelBase(ViewModelEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public abstract ViewModelType ViewModelType { get; }

    public bool CanHandle(ViewModelType type) => type == ViewModelType;

    public ValueTask HandleAsync(CommandPayload payload)
        => payload.Type.Equals(DefaultActionType.Init, StringComparison.OrdinalIgnoreCase)
            ? HandleInitAsync()
            : !Enum.TryParse<TAction>(payload.Type, true, out var action)
            ? ValueTask.CompletedTask
            : HandleActionAsync(action, payload.Payload);

    protected void Dispatch<T>(EventPayload<T> payload, JsonTypeInfo<EventMessage<T>> jsonTypeInfo)
        => _dispatcher.Dispatch(ViewModelType, payload, jsonTypeInfo);

    protected abstract ValueTask HandleActionAsync(TAction action, JsonElement? payload);

    protected abstract ValueTask HandleInitAsync();

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
