using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Messages;

#region Event
public record MessageBoxResultEvent(Guid CommandId, MessageBoxResultEvent.ResultType Payload)
    : EventResultMessage<MessageBoxResultEvent.ResultType>(CommandId, "receive:messageBox", Payload)
{
    [JsonConverter(typeof(JsonStringEnumConverter<ResultType>))]
    public enum ResultType
    {
        Ok,
        Cancel,
        Yes,
        No
    }
}
#endregion

#region Command
public enum WindowCommandType
{
    MessageBox
}

public record MessageBoxCommand(
    string? Title, string Message, MessageBoxCommand.ButtonsType? Buttons, MessageBoxCommand.IconType? Icon)
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
#endregion