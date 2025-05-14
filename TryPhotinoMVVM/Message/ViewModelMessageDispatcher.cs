using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Photino.NET;
using TryPhotinoMVVM.Constants;

namespace TryPhotinoMVVM.Message;

public class ViewModelMessageDispatcher(PhotinoWindow window)
{
    public void Dispatch<TPayload>(
        ViewModelType type,
        TPayload payload,
        JsonTypeInfo<StateMessage<TPayload>> jsonTypeInfo
    )
    {
        var message = new StateMessage<TPayload>(type, payload);
        var json = JsonSerializer.Serialize(message, jsonTypeInfo);
        window.SendWebMessage(json);
    }

    public void DispatchEvent<TPayload>(
        ViewModelType type,
        EventMessagePayload<TPayload> payload,
        JsonTypeInfo<EventMessage<TPayload>> jsonTypeInfo
    )
    {
        var message = new EventMessage<TPayload>($"{type}_Event", payload);
        var json = JsonSerializer.Serialize(message, jsonTypeInfo);
        window.SendWebMessage(json);
    }
}
