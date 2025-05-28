using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public abstract class ViewModelBase<TAction>(EventDispatcher dispatcher) : IViewModel
    where TAction : struct, Enum
{
    public abstract ViewModelType ViewModelType { get; }

    public bool CanHandle(ViewModelType type) => type == ViewModelType;

    public ValueTask HandleAsync(string command, JsonElement? payload)
        => command.Equals(DefaultActionType.Init.ToString(), StringComparison.OrdinalIgnoreCase)
            ? HandleInitAsync()
            : Enum.TryParse<TAction>(command, true, out var action)
            ? HandleActionAsync(action, payload)
            : ValueTask.CompletedTask;

    protected void Dispatch<T>(EventMessage<T> message, JsonTypeInfo<EventMessage<T>> jsonTypeInfo)
        => dispatcher.Dispatch(message, jsonTypeInfo);

    protected abstract ValueTask HandleActionAsync(TAction action, JsonElement? payload);

    protected abstract ValueTask HandleInitAsync();
}
