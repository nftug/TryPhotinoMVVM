using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Photino.NET;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.Views;

public class ViewModelEventDispatcher(PhotinoWindow window)
{
    public void Dispatch<TPayload>(
        ViewModelType type,
        EventPayload<TPayload> payload,
        JsonTypeInfo<EventMessage<TPayload>> jsonTypeInfo
    )
    {
        var message = new EventMessage<TPayload>(type, payload);
        var json = JsonSerializer.Serialize(message, jsonTypeInfo);
        window.Invoke(() => window.SendWebMessage(json));
    }
}
