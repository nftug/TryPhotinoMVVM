using System.Text.Json;

namespace TryPhotinoMVVM.Message;

public record ViewModelMessage<TPayload>(ViewModelType Type, TPayload Payload);

public record CommandMessage(ViewModelType Type, CommandPayload? Payload);

public record CommandPayload(string Type, JsonElement? Payload);
