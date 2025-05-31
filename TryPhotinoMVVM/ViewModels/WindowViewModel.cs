using System.Text.Json;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.ViewModels.Abstractions;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM.ViewModels;

public class WindowViewModel(EventDispatcher eventDispatcher, PhotinoWindowInstance windowInstance)
    : ViewModelBase<WindowCommandType>(eventDispatcher)
{
    public override void HandleInit() { }

    protected override ValueTask HandleActionAsync(WindowCommandType action, JsonElement? payload, Guid? commandId)
        => (action, payload) switch
        {
            (WindowCommandType.MessageBox, { } p) =>
                p.HandlePayloadSync(JsonContext.Default.MessageBoxCommand, p => ShowMessageBox(p, commandId)),
            _ => ValueTask.CompletedTask
        };

    private void ShowMessageBox(MessageBoxCommand command, Guid? commandId)
    {
        if (windowInstance.Value is not { } window)
            throw new InvalidOperationException("Window instance is not ready");

        string title = command.Title ?? EnvironmentConstants.AppName;

        var buttons = command.Buttons switch
        {
            MessageBoxCommand.ButtonsType.Ok => PhotinoDialogButtons.Ok,
            MessageBoxCommand.ButtonsType.OkCancel => PhotinoDialogButtons.OkCancel,
            MessageBoxCommand.ButtonsType.YesNo => PhotinoDialogButtons.YesNo,
            MessageBoxCommand.ButtonsType.YesNoCancel => PhotinoDialogButtons.YesNoCancel,
            _ => PhotinoDialogButtons.Ok,
        };

        var icon = command.Icon switch
        {
            MessageBoxCommand.IconType.Info => PhotinoDialogIcon.Info,
            MessageBoxCommand.IconType.Warning => PhotinoDialogIcon.Warning,
            MessageBoxCommand.IconType.Error => PhotinoDialogIcon.Error,
            MessageBoxCommand.IconType.Question => PhotinoDialogIcon.Question,
            _ => PhotinoDialogIcon.Info
        };

        var dialogResult = window.ShowMessage(title, command.Message, buttons, icon);
        var commandResult = dialogResult switch
        {
            PhotinoDialogResult.Ok => MessageBoxResultEvent.ResultType.Ok,
            PhotinoDialogResult.Cancel => MessageBoxResultEvent.ResultType.Cancel,
            PhotinoDialogResult.Yes => MessageBoxResultEvent.ResultType.Yes,
            PhotinoDialogResult.No => MessageBoxResultEvent.ResultType.No,
            _ => MessageBoxResultEvent.ResultType.Ok
        };

        if (commandId != null)
        {
            var eventPayload = new MessageBoxResultEvent(commandId.Value, commandResult);

            Dispatch(new(commandId.Value, "messageBox", commandResult),
                JsonContext.Default.EventResultMessageResultType);
        }
    }
}
