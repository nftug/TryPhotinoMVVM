using Microsoft.Extensions.Logging;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.Views;

public class ErrorHandlerService(PhotinoWindowInstance window, EventDispatcher dispatcher, ILogger<Program> logger)
{
    public void HandleError(Exception exception)
    {
        logger.LogError(exception, "Error!: {Message}", exception.Message);

        ErrorMessage payload =
             exception is ViewModelException vmEx
             ? new(vmEx.Type, vmEx.Message) : new(null, exception.Message);

        dispatcher.Dispatch(
           ViewModelType.Error, new ErrorEvent(payload), JsonContext.Default.EventMessageErrorMessage);

        window.Value?.ShowMessage(
            EnvironmentConstants.AppName, exception.Message, PhotinoDialogButtons.Ok, PhotinoDialogIcon.Error);
    }
}
