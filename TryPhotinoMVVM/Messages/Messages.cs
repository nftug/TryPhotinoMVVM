using System.Text.Json;

namespace TryPhotinoMVVM.Messages;

#region Events
public record EventMessage<TPayload>(ViewModelType Type, EventPayload<TPayload> Payload);

public record EventPayload<TPayload>(string Type, TPayload? Payload);

public record EventEmptyPayload(string Type)
    : EventPayload<EventEmptyPayload.DummyPayload>(Type, new())
{
    public record DummyPayload;
}
#endregion

#region Actions
public record CommandMessage(ViewModelType Type, CommandPayload Payload);

public record CommandPayload(string Type, JsonElement? Payload);

public enum DefaultActionType
{
    Init
}
#endregion
