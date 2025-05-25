using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Photino.NET;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.Views;

public class EventDispatcher(PhotinoWindowInstance _window)
{
    public void Dispatch<TPayload>(
        ViewModelType type,
        EventPayload<TPayload> payload,
        JsonTypeInfo<EventMessage<TPayload>> jsonTypeInfo
    )
    {
        if (_window.Value is not { } window) return;

        var message = new EventMessage<TPayload>(type, payload);
        var json = JsonSerializer.Serialize(message, jsonTypeInfo);
        window.Invoke(() => window.SendWebMessage(json));
    }
}
