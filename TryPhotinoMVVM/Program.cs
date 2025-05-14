using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.Utils;
using TryPhotinoMVVM.ViewModels;

public class Program
{
    [STAThread]
    public static void Main()
    {
        string embeddedAppUrlHost = OperatingSystem.IsWindows() ? "http://localhost" : "app://localhost/";
        string embeddedAppUrl = embeddedAppUrlHost + $"?hash={typeof(Program).Assembly.GetBuildDateHash()}";
        string appUrl = EnvironmentConstants.IsDebugMode ? "http://localhost:5173/" : embeddedAppUrl;

        var window = new PhotinoWindow();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(window)
            .AddSingleton<ViewModelEventDispatcher>()
            .AddSingleton<CommandMessageDispatcher>(sp =>
                new CommandMessageDispatcher()
                    .Register(sp.GetRequiredService<CounterViewModel>()))
            .AddSingleton<CounterViewModel>()
            .AddLogging(builder => builder.AddConsole())
            .BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        window
            .SetTitle("Photino MVVM Counter")
            .SetUseOsDefaultSize(false)
            .SetSize(new(1145, 840))
            .Center()
            .RegisterCustomSchemeHandler(new Uri(embeddedAppUrl).Scheme, AppSchemeHandler.Handle)
            .LoadRawString($"""<meta http-equiv="refresh" content="0; URL='{appUrl}'" />""")
            .RegisterWebMessageReceivedHandler(async (sender, messageJson) =>
            {
                try
                {
                    var dispatcher = serviceProvider.GetRequiredService<CommandMessageDispatcher>();
                    await dispatcher.DispatchAsync(messageJson);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());

                    ErrorMessage payload =
                        ex is ViewModelException vmEx
                        ? new(vmEx.Type, vmEx.Message) : new(null, ex.Message);

                    var dispatcher = serviceProvider.GetRequiredService<ViewModelEventDispatcher>();
                    dispatcher.Dispatch(
                       ViewModelType.Error, new ErrorEvent(payload), JsonContext.Default.EventMessageErrorMessage);

                    window.ShowMessage(
                        $"Error from {payload.Type}", ex.Message, PhotinoDialogButtons.Ok, PhotinoDialogIcon.Error);
                }
            });

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            var ex = e.ExceptionObject as Exception;
            logger.LogError(ex?.ToString());
            Environment.Exit(1);
        };

        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            logger.LogError(e.Exception.ToString());
            e.SetObserved();
        };

        window.WaitForClose();
    }
}
