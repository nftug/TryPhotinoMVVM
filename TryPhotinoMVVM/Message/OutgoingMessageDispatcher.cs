using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Photino.NET;
using TryPhotinoMVVM.Constants;

namespace TryPhotinoMVVM.Message;

public class OutgoingMessageDispatcher(PhotinoWindow window)
{
    public void Dispatch<TPayload>(ViewModelType type, TPayload? payload)
    {
        var message = new OutgoingMessage<TPayload>(type, payload);

        JsonTypeInfo typeInfo = type switch
        {
            ViewModelType.Counter => JsonContext.Default.OutgoingMessageCounterOutgoingPayload,
            _ => throw new InvalidOperationException("OutgoingMessageDispatcher: TypeInfo is not found"),
        };

        var json = JsonSerializer.Serialize(message, typeInfo);
        window.SendWebMessage(json);
    }
}
