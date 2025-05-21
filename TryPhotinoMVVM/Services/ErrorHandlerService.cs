using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Models;

namespace TryPhotinoMVVM.Services;

public class ErrorHandlerService(
    PhotinoWindow window, ILogger<Program> logger, ViewModelEventDispatcher dispatcher)
{
    public void HandleError(Exception exception)
    {
        logger.LogError(exception.ToString());

        ErrorMessage payload =
            exception is ViewModelException vmEx
            ? new(vmEx.Type, vmEx.Message) : new(null, exception.Message);

        dispatcher.Dispatch(
           ViewModelType.Error, new ErrorEvent(payload), JsonContext.Default.EventMessageErrorMessage);

        window.ShowMessage(
            EnvironmentConstants.AppName, exception.Message, PhotinoDialogButtons.Ok, PhotinoDialogIcon.Error);
    }
}
