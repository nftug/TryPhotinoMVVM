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
        JsonTypeInfo<ViewModelMessage<TPayload>> jsonTypeInfo
    )
    {
        var message = new ViewModelMessage<TPayload>(type, payload);
        var json = JsonSerializer.Serialize(message, jsonTypeInfo);
        window.SendWebMessage(json);
    }
}
