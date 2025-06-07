using System.Text.Json.Serialization;
using TryPhotinoMVVM.Enums;

namespace TryPhotinoMVVM.Dtos.Application.Commands;

[JsonConverter(typeof(JsonStringEnumConverter<AppActionType>))]
public enum AppActionType
{
    Init,
    Leave
}

public record InitCommandPayload(ViewModelType Type);
