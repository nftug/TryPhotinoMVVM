using System.Text.Json;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.Views;

public class CommandDispatcher
{
    private readonly List<IViewModel> _handlers = new();

    public CommandDispatcher Register(IViewModel handler)
    {
        _handlers.Add(handler);
        return this;
    }

    public async ValueTask DispatchAsync(string json)
    {
        var msg = JsonSerializer.Deserialize(json, JsonContext.Default.CommandMessage);
        if (msg == null) return;

        var handler = _handlers.FirstOrDefault(h => h.CanHandle(msg.Type));
        if (handler != null) await handler.HandleAsync(msg.Payload);
    }
}