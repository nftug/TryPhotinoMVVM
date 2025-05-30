using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Utils;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM.Services;

public class AppService(
    PhotinoWindowInstance windowInstance, CommandDispatcher dispatcher, ErrorHandlerService errorHandler)
{
    private bool? _isClosing;
    private PhotinoWindow _window = null!;

    public void Run(AppContainer container)
    {
        dispatcher.InjectContainer(container);

        _window = new PhotinoWindow();

        string embeddedAppUrlHost = OperatingSystem.IsWindows() ? "http://localhost" : "app://localhost/";
        string embeddedAppUrl = embeddedAppUrlHost + $"?hash={typeof(Program).Assembly.GetBuildDateHash()}";
        string appUrl = EnvironmentConstants.IsDebugMode ? "http://localhost:5173/" : embeddedAppUrl;

        _window
            .SetTitle(EnvironmentConstants.AppName)
            .SetUseOsDefaultSize(false)
            .SetSize(new(1145, 840))
            .Center()
            .SetContextMenuEnabled(!EnvironmentConstants.IsDebugMode)
            .LoadRawString($"""<meta http-equiv="refresh" content="0; URL='{appUrl}'" />""")
            .RegisterWebMessageReceivedHandler(HandleWebMessageReceived)
            .RegisterWindowCreatedHandler(HandleWindowCreated)
            .RegisterWindowClosingHandler(HandleWindowClosing);

        if (!EnvironmentConstants.IsDebugMode)
        {
            _window.RegisterCustomSchemeHandler(new Uri(embeddedAppUrl).Scheme, AppSchemeHandler.Handle);
        }

        _window.WaitForClose();
    }

    private async void HandleWebMessageReceived(object? sender, string messageJson)
    {
        try
        {
            await dispatcher.DispatchAsync(messageJson);
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
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
