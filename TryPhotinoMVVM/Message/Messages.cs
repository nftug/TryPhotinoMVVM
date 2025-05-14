using System.Text.Json;

namespace TryPhotinoMVVM.Message;

public record ViewModelMessage<TPayload>(ViewModelType Type, TPayload Payload);

public record EventMessage<TPayload>(string Type, EventMessagePayload<TPayload> Payload);

public record EventMessagePayload<TPayload>(string Type, TPayload? Payload);

public record CommandMessage(ViewModelType Type, CommandMessagePayload Payload);

public record CommandMessagePayload(string Type, JsonElement? Payload);