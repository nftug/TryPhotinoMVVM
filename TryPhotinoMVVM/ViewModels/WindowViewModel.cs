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
                p.HandlePayloadSync(JsonContext.Default.MessageBoxCommandPayload, p => ShowMessageBox(p, commandId)),
            _ => ValueTask.CompletedTask
        };

    private void ShowMessageBox(MessageBoxCommandPayload command, Guid? commandId)
    {
        if (windowInstance.Value is not { } window)
            throw new InvalidOperationException("Window instance is not ready");

        string title = command.Title ?? EnvironmentConstants.AppName;

        var buttons = command.Buttons switch
        {
            MessageBoxCommandPayload.ButtonsType.Ok => PhotinoDialogButtons.Ok,
            MessageBoxCommandPayload.ButtonsType.OkCancel => PhotinoDialogButtons.OkCancel,
            MessageBoxCommandPayload.ButtonsType.YesNo => PhotinoDialogButtons.YesNo,
            MessageBoxCommandPayload.ButtonsType.YesNoCancel => PhotinoDialogButtons.YesNoCancel,
            _ => PhotinoDialogButtons.Ok,
        };

        var icon = command.Icon switch
        {
            MessageBoxCommandPayload.IconType.Info => PhotinoDialogIcon.Info,
            MessageBoxCommandPayload.IconType.Warning => PhotinoDialogIcon.Warning,
            MessageBoxCommandPayload.IconType.Error => PhotinoDialogIcon.Error,
            MessageBoxCommandPayload.IconType.Question => PhotinoDialogIcon.Question,
            _ => PhotinoDialogIcon.Info
        };

        var dialogResult = window.ShowMessage(title, command.Message, buttons, icon);
        var commandResult = dialogResult switch
        {
            PhotinoDialogResult.Ok => MessageBoxResultEvent.MessageBoxResultType.Ok,
            PhotinoDialogResult.Cancel => MessageBoxResultEvent.MessageBoxResultType.Cancel,
            PhotinoDialogResult.Yes => MessageBoxResultEvent.MessageBoxResultType.Yes,
            PhotinoDialogResult.No => MessageBoxResultEvent.MessageBoxResultType.No,
            _ => MessageBoxResultEvent.MessageBoxResultType.Ok
        };

        if (commandId != null)
        {
            var eventPayload = new MessageBoxResultEvent(commandResult, commandId.Value);
            Dispatch(eventPayload, JsonContext.Default.EventMessageMessageBoxResultType);
        }
    }
}
