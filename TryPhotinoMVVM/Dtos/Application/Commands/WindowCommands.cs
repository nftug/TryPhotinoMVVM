using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Dtos.Application.Commands;

public enum WindowCommandType
{
    MessageBox
}

public record MessageBoxCommandPayload(
    string? Title, string Message, MessageBoxCommandPayload.ButtonsType? Buttons, MessageBoxCommandPayload.IconType? Icon)
{
    [JsonConverter(typeof(JsonStringEnumConverter<ButtonsType>))]
    public enum ButtonsType
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel
    }

    [JsonConverter(typeof(JsonStringEnumConverter<IconType>))]
    public enum IconType
    {
        Info,
        Warning,
        Error,
        Question
    }
}