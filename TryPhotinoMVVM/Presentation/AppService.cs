using Photino.NET;
using TryPhotinoMVVM.Composition;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Presentation.Dispatchers;

namespace TryPhotinoMVVM.Presentation;

public class AppService(
    AppContainerInstance containerInstance,
    PhotinoWindowInstance windowInstance,
    CommandDispatcher dispatcher,
    ErrorHandlerService errorHandler,
    AppSchemeHandler appSchemeHandler
)
{
    private bool? _isClosing;
    private PhotinoWindow _window = null!;

    public void Run(AppContainer container)
    {
        containerInstance.Inject(container);

        _window = new PhotinoWindow();

        string appUrl =
            EnvironmentConstants.IsDebugMode ? "http://localhost:5173/"
            : OperatingSystem.IsWindows() ? "http://localhost" : "app://localhost/";

        _window
            .SetTitle(EnvironmentConstants.AppName)
            .SetUseOsDefaultSize(false)
            .SetSize(new(1400, 1024))
            .Center()
            .SetContextMenuEnabled(EnvironmentConstants.IsDebugMode)
            .SetDevToolsEnabled(EnvironmentConstants.IsDebugMode)
            .LoadRawString($"""<meta http-equiv="refresh" content="0; URL='{appUrl}'" />""")
            .RegisterWebMessageReceivedHandler(HandleWebMessageReceived)
            .RegisterWindowCreatedHandler(HandleWindowCreated)
            .RegisterWindowClosingHandler(HandleWindowClosing);

        if (!EnvironmentConstants.IsDebugMode)
        {
            _window.RegisterCustomSchemeHandler(new Uri(appUrl).Scheme, appSchemeHandler.Handle);
        }

        _window.WaitForClose();
    }

    private async void HandleWebMessageReceived(object? sender, string messageJson)
    {
        await dispatcher.DispatchAsync(messageJson);
    }

    private void HandleWindowCreated(object? sender, EventArgs e)
    {
        windowInstance.Inject(_window);

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            errorHandler.HandleError((e.ExceptionObject as Exception)!);
            Environment.Exit(1);
        };
        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            errorHandler.HandleError(e.Exception);
            e.SetObserved();
        };
    }

    private bool HandleWindowClosing(object? sender, EventArgs e)
    {
        if (_isClosing is { } isClosing)
        {
            _isClosing = null;
            return !isClosing;
        }

        Task.Run(() =>
        {
            // NOTE: ここに何かしらの終了処理を非同期で入れる
            _isClosing = true;
            _window.Close();
        });

        return true;
    }
}
