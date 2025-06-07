using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Dtos.Abstractions.Events;

namespace TryPhotinoMVVM.Presentation.Dispatchers;

public class EventDispatcher(PhotinoWindowInstance _window)
{
    public void Dispatch<TPayload>(
        EventMessage<TPayload> message,
        JsonTypeInfo<EventMessage<TPayload>> jsonTypeInfo
    )
    {
        if (_window.Value is not { } window) return;
        if (message.ViewId == default) return;
        window.SendWebMessage(JsonSerializer.Serialize(message, jsonTypeInfo));
    }
}
