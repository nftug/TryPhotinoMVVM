using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Models.Abstractions;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public abstract class ViewModelBase<TAction>(EventDispatcher dispatcher) : DisposableBase, IViewModel
    where TAction : struct, Enum
{
    protected Guid ViewId { get; private set; }

    public ValueTask HandleAsync(string command, JsonElement? payload)
        => Enum.TryParse<TAction>(command, true, out var action)
            ? HandleActionAsync(action, payload)
            : ValueTask.CompletedTask;

    protected void Dispatch<T>(EventMessage<T> message, JsonTypeInfo<EventMessage<T>> jsonTypeInfo)
        => dispatcher.Dispatch(message with { ViewId = ViewId }, jsonTypeInfo);

    protected abstract ValueTask HandleActionAsync(TAction action, JsonElement? payload);

    public abstract void HandleInit();

    public IViewModel SetViewId(Guid viewId)
    {
        ViewId = viewId;
        return this;
    }
}
