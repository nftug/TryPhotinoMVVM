using Microsoft.Extensions.Logging;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Exceptions;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.Views;

public class ErrorHandlerService(
    PhotinoWindowInstance window, EventDispatcher eventDispatcher, ILogger<Program> logger)
{
    public void HandleError(Exception exception)
    {
        logger.LogError(exception, "Error!: {Message}", exception.Message);

        if (exception is ViewModelException vmException)
        {
            var errorEvent = new ViewModelErrorEvent(new(exception.Message, exception.ToString()))
            {
                ViewId = vmException.ViewId
            };
            eventDispatcher.Dispatch(errorEvent, JsonContext.Default.EventMessageViewModelError);
        }

        window.Value?.ShowMessage(
            EnvironmentConstants.AppName, exception.Message, PhotinoDialogButtons.Ok, PhotinoDialogIcon.Error);
    }
}
