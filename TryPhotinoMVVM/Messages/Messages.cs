using System.Text.Json;
using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Messages;

#region Events
public record EventMessage<TPayload>(string Event, TPayload? Payload)
{
    public Guid ViewId { get; init; }
}

public record EventEmptyMessage(string Event)
    : EventMessage<EventEmptyMessage.DummyPayload>(Event, new())
{
    public record DummyPayload;
}

public record EventResultMessage<TPayload>(Guid CommandId, string CommandName, TPayload? Payload)
    : EventMessage<TPayload>($"receive:{CommandName}", Payload);
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
