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
#endregion

#region Actions
public record CommandMessage(Guid ViewId, string Command, JsonElement? Payload);

[JsonConverter(typeof(JsonStringEnumConverter<DefaultActionType>))]
public enum DefaultActionType
{
    Init,
    Leave
}

public record InitCommandPayload(ViewModelType Type);
#endregion
