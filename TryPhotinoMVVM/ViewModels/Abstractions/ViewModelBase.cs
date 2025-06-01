using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Models.Abstractions;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public abstract class ViewModelBase<TCommandType>(EventDispatcher dispatcher) : DisposableBase, IViewModel
    where TCommandType : struct, Enum
{
    protected Guid ViewId { get; private set; }

    public ValueTask HandleAsync(CommandMessage message)
        => Enum.TryParse<TCommandType>(message.Command, true, out var action)
            ? HandleActionAsync(action, message.Payload, message.CommandId)
            : ValueTask.CompletedTask;

    protected void Dispatch<T>(EventMessage<T> message, JsonTypeInfo<EventMessage<T>> jsonTypeInfo)
        => dispatcher.Dispatch(message with { ViewId = ViewId }, jsonTypeInfo);

    protected abstract ValueTask HandleActionAsync(TCommandType action, JsonElement? payload, Guid? commandId);

    public abstract void HandleInit();

    public void SetViewId(Guid viewId) => ViewId = viewId;
}
