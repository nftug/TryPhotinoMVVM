using System.Text.Json.Serialization;
using TryPhotinoMVVM.Dtos.Abstractions.Events;

namespace TryPhotinoMVVM.Dtos.Application.Events;

public record MessageBoxResultEvent : EventMessage<MessageBoxResultEvent.MessageBoxResultType>
{
    public MessageBoxResultEvent(MessageBoxResultType payload, Guid commandId)
        : base(payload, commandId, "messageBox") { }

    [JsonConverter(typeof(JsonStringEnumConverter<MessageBoxResultType>))]
    public enum MessageBoxResultType
    {
        Ok,
        Cancel,
        Yes,
        No
    }
}
