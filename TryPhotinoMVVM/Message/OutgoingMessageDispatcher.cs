using System.Text.Json;
using Photino.NET;
using TryPhotinoMVVM.Constants;

namespace TryPhotinoMVVM.Message;

public class OutgoingMessageDispatcher(PhotinoWindow window)
{
    public void Dispatch<TPayload>(ViewModelType type, TPayload? payload)
    {
        var message = new OutgoingMessage<TPayload>(type, payload);
        var json = JsonSerializer.Serialize(message, JsonSerializerOptions.Web);
        window.SendWebMessage(json);
    }
}
