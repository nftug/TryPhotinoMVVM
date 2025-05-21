using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Utils;

namespace TryPhotinoMVVM.Services;

public class WindowManagerService(
    PhotinoWindow window, ErrorHandlerService errorHandler, IServiceProvider serviceProvider)
{
    private bool _isClosing = false;

    public void Run()
    {
        string embeddedAppUrlHost = OperatingSystem.IsWindows() ? "http://localhost" : "app://localhost/";
        string embeddedAppUrl = embeddedAppUrlHost + $"?hash={typeof(Program).Assembly.GetBuildDateHash()}";
        string appUrl = EnvironmentConstants.IsDebugMode ? "http://localhost:5173/" : embeddedAppUrl;

        window
            .SetTitle(EnvironmentConstants.AppName)
            .SetUseOsDefaultSize(false)
            .SetSize(new(1145, 840))
            .Center()
            .SetContextMenuEnabled(false)
            .RegisterCustomSchemeHandler(new Uri(embeddedAppUrl).Scheme, AppSchemeHandler.Handle)
            .LoadRawString($"""<meta http-equiv="refresh" content="0; URL='{appUrl}'" />""")
            .RegisterWebMessageReceivedHandler(HandleWebMessageReceived)
            .RegisterWindowCreatedHandler(HandleWindowCreated)
            .RegisterWindowClosingHandler(HandleWindowClosing);

        window.WaitForClose();
    }

    private async void HandleWebMessageReceived(object? sender, string messageJson)
    {
        try
        {
            var dispatcher = serviceProvider.GetRequiredService<CommandMessageDispatcher>();
            await dispatcher.DispatchAsync(messageJson);
        }
        catch (Exception ex)
        {
            var errorHandler = serviceProvider.GetRequiredService<ErrorHandlerService>();
            errorHandler.HandleError(ex);
        }
    }

    private void HandleWindowCreated(object? sender, EventArgs e)
    {
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
        if (_isClosing) return false;

        Task.Run(() =>
        {
            // NOTE: ここに何かしらの終了処理を非同期で入れる
            _isClosing = true;
            window.Invoke(() => window.Close());
        });

        return true;
    }
}
