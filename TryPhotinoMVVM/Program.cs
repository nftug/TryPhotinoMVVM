using System.Drawing;
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
        var windowSize = new Size(1145, 840);

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
            .SetSize(windowSize)
            .Center()
            .RegisterCustomSchemeHandler(new Uri(embeddedAppUrl).Scheme, AppSchemeHandler.Handle)
            .LoadRawString($"""<meta http-equiv="refresh" content="0; URL='{appUrl}'" />""")
            .RegisterWebMessageReceivedHandler(async (sender, messageJson) =>
            {
                var dispatcher = serviceProvider.GetRequiredService<CommandMessageDispatcher>();
                await dispatcher.DispatchAsync(messageJson);
            })
            .RegisterWindowCreatedHandler((_, _) =>
            {
                // MacではWindowの生成後でのみサイズの変更が可能
                window.SetSize(windowSize);
            });

        window.WaitForClose();
    }
}
