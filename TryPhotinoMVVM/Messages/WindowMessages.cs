using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Messages;

#region Event
public record MessageBoxResultEvent : EventMessage<MessageBoxResultEvent.MessageBoxResultType>
{
    public MessageBoxResultEvent(MessageBoxResultType payload, Guid commandId)
        : base(payload, commandId, "messageBox") { }

    [JsonConverter(typeof(JsonStringEnumConverter<MessageBoxResultType>))]
    public enum MessageBoxResultType
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