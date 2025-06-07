using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace BrowserBridge.Photino;

public class PhotinoEventDispatcher(PhotinoWindowInstance _window) : IEventDispatcher
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
