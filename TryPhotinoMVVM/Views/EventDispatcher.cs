using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.Views;

public class EventDispatcher(PhotinoWindowInstance _window)
{
    public void Dispatch<TPayload>(
        EventMessage<TPayload> message,
        JsonTypeInfo<EventMessage<TPayload>> jsonTypeInfo
    ) => DispatchInternal(message.ViewId, () => JsonSerializer.Serialize(message, jsonTypeInfo));

    public void Dispatch<TPayload>(
        EventResultMessage<TPayload> message,
        JsonTypeInfo<EventResultMessage<TPayload>> jsonTypeInfo
    ) => DispatchInternal(message.ViewId, () => JsonSerializer.Serialize(message, jsonTypeInfo));

    private void DispatchInternal(Guid viewId, Func<string> serialize)
    {
        if (_window.Value is not { } window) return;
        if (viewId == default) return;
        window.SendWebMessage(serialize());
    }
}
