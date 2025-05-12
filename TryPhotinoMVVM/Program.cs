using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Message;
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
            .AddSingleton<ViewModelMessageDispatcher>()
            .AddSingleton<CommandMessageDispatcher>(sp =>
                new CommandMessageDispatcher()
                    .Register(sp.GetRequiredService<CounterViewModel>()))
            .AddSingleton<CounterViewModel>()
            .BuildServiceProvider();

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
                    Console.WriteLine(ex.ToString());

                    var dispatcher = serviceProvider.GetRequiredService<ViewModelMessageDispatcher>();
                    dispatcher.Dispatch(
                       ViewModelType.Error, new(ex.Message), JsonContext.Default.ViewModelMessageErrorMessage);
                }
            });

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            var ex = e.ExceptionObject as Exception;
            Console.WriteLine(ex?.ToString());
            Environment.Exit(1);
        };

        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            Console.WriteLine(e.Exception.ToString());
            e.SetObserved();
        };

        window.WaitForClose();
    }
}
