using System.Text.Json;
using TryPhotinoMVVM.Constants;

namespace TryPhotinoMVVM.Message;

public class IncomingMessageDispatcher
{
    private readonly List<IMessageHandler> _handlers = new();

    public IncomingMessageDispatcher Register(IMessageHandler handler)
    {
        _handlers.Add(handler);
        return this;
    }

    public void Dispatch(string json)
    {
        var msg = JsonSerializer.Deserialize<IncomingMessage>(json, JsonSerializerOptions.Web);
        if (msg == null) return;

        var handler = _handlers.FirstOrDefault(h => h.CanHandle(msg.Type));
        handler?.Handle(msg.Payload);
    }
}