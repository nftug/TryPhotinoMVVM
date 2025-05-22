using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public abstract class ViewModelBase<TAction>(ViewModelEventDispatcher dispatcher) : IViewModel
    where TAction : struct, Enum
{
    public abstract ViewModelType ViewModelType { get; }

    public bool CanHandle(ViewModelType type) => type == ViewModelType;

    public ValueTask HandleAsync(CommandPayload payload)
        => payload.Type.Equals(DefaultActionType.Init.ToString(), StringComparison.OrdinalIgnoreCase)
            ? HandleInitAsync()
            : !Enum.TryParse<TAction>(payload.Type, true, out var action)
            ? ValueTask.CompletedTask
            : HandleActionAsync(action, payload.Payload);

    protected void Dispatch<T>(EventPayload<T> payload, JsonTypeInfo<EventMessage<T>> jsonTypeInfo)
        => dispatcher.Dispatch(ViewModelType, payload, jsonTypeInfo);

    protected abstract ValueTask HandleActionAsync(TAction action, JsonElement? payload);

    protected abstract ValueTask HandleInitAsync();
}
