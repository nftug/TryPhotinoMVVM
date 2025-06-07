using System.Text.Json;

namespace TryPhotinoMVVM.Dtos.Abstractions.Commands;

public record CommandMessage(Guid ViewId, Guid? CommandId, string Command, JsonElement? Payload);