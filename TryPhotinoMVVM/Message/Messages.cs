using System.Text.Json;

namespace TryPhotinoMVVM.Message;

public record OutgoingMessage<TPayload>(ViewModelType Type, TPayload? Payload);

public record IncomingMessage(ViewModelType Type, IncomingSubMessage? Payload);

public record IncomingSubMessage(string Type, JsonElement? Payload);
