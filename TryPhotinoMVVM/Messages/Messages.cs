using System.Text.Json;

namespace TryPhotinoMVVM.Messages;

#region Events
public record EventMessage<TPayload>(ViewModelType Type, string Event, TPayload? Payload);

public record EventEmptyMessage(ViewModelType Type, string Event)
    : EventMessage<EventEmptyMessage.DummyPayload>(Type, Event, new())
{
    public record DummyPayload;
}
#endregion

#region Actions
public record CommandMessage(ViewModelType Type, string Command, JsonElement? Payload);

public enum DefaultActionType
{
    Init
}
#endregion
