using System.Text.Json;

namespace TryPhotinoMVVM.Message;

public record EventMessage<TPayload>(ViewModelType Type, EventPayload<TPayload> Payload);

public record EventPayload<TPayload>(string Type, TPayload? Payload);

public record EventEmptyPayload(string Type)
    : EventPayload<EventEmptyPayload.DummyPayload>(Type, new())
{
    public record DummyPayload;
}

public record CommandMessage(ViewModelType Type, CommandPayload Payload);

public record CommandPayload(string Type, JsonElement? Payload);
