using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using R3;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Models.Abstractions;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public abstract class ViewModelBase<TCommandType> : DisposableBase, IViewModel
    where TCommandType : struct, Enum
{
    private readonly EventDispatcher _dispatcher;

    private readonly ReactiveProperty<Guid> _viewId = new();
    protected ReadOnlyReactiveProperty<Guid> ViewId { get; }

    public ViewModelBase(EventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _viewId.AddTo(Disposable);
        ViewId = _viewId.ToReadOnlyReactiveProperty().AddTo(Disposable);
    }

    public ValueTask HandleAsync(CommandMessage message)
        => Enum.TryParse<TCommandType>(message.Command, true, out var action)
            ? HandleActionAsync(action, message.Payload, message.CommandId)
            : ValueTask.CompletedTask;

    protected Observable<T> NotifyObservable<T>(Observable<T> observable)
        => _viewId.Where(v => v != default).CombineLatest(observable, (_, v) => v);

    protected void Dispatch<T>(EventMessage<T> message, JsonTypeInfo<EventMessage<T>> jsonTypeInfo)
        => _dispatcher.Dispatch(message with { ViewId = _viewId.Value }, jsonTypeInfo);

    protected abstract ValueTask HandleActionAsync(TCommandType action, JsonElement? payload, Guid? commandId);

    public void SetViewId(Guid viewId) => _viewId.Value = viewId;
}
