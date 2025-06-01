using System.Text.Json;
using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Messages;

#region Events
public record EventMessage<TPayload>(string Event, TPayload? Payload)
{
    public Guid ViewId { get; init; }
    public Guid? CommandId { get; init; }
    public string? CommandName { get; init; }

    public EventMessage(TPayload? payload, Guid commandId, string commandName)
        : this($"receive:{commandName}", payload)
    {
        CommandId = commandId;
        CommandName = commandName;
    }
}

public record EventEmptyMessage(string Event)
    : EventMessage<EventEmptyMessage.DummyPayload>(Event, new())
{
    public record DummyPayload;
}

public record ViewModelErrorEvent(ViewModelErrorEvent.ViewModelError Payload)
    : EventMessage<ViewModelErrorEvent.ViewModelError>("error", Payload)
{
    public record ViewModelError(string Message, string Details);
}
#endregion

#region Actions
public record CommandMessage(Guid ViewId, Guid? CommandId, string Command, JsonElement? Payload);

[JsonConverter(typeof(JsonStringEnumConverter<AppActionType>))]
public enum AppActionType
{
    Init,
    Leave
}

public record InitCommandPayload(ViewModelType Type);
#endregion
