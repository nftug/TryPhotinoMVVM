using System.Text.Json;
using System.Text.Json.Serialization;
using TryPhotinoMVVM.Enums;

namespace TryPhotinoMVVM.Dtos.Abstractions.Events;

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

public record DummyEventPayload;


#endregion

#region Actions



#endregion
