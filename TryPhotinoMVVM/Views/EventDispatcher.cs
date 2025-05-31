using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.Views;

public class EventDispatcher(PhotinoWindowInstance _window)
{
    public void Dispatch<TPayload>(
        EventMessage<TPayload> message,
        JsonTypeInfo<EventMessage<TPayload>> jsonTypeInfo
    )
    {
        if (_window.Value is not { } window) return;
        if (message.ViewId == default) return;

        var json = JsonSerializer.Serialize(message, jsonTypeInfo);
        window.SendWebMessage(json);
    }
}
