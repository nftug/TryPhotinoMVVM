using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Utils;
using TryPhotinoMVVM.ViewModels;

class Program
{
    [STAThread]
    static void Main()
    {
        string embeddedAppUrlHost = OperatingSystem.IsWindows() ? "http://localhost" : "app://localhost/";
        string embeddedAppUrl = embeddedAppUrlHost + $"?hash={typeof(Program).Assembly.GetBuildDateHash()}";
        string appUrl = EnvironmentConstants.IsDebugMode ? "http://localhost:5173/" : embeddedAppUrl;

        var window = new PhotinoWindow();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(window)
            .AddSingleton<ViewModelMessageDispatcher>()
            .AddSingleton<CommandMessageDispatcher>(sp =>
            {
                var dispatcher = new CommandMessageDispatcher();
                return dispatcher.Register(sp.GetRequiredService<CounterViewModel>());
            })
            .AddSingleton<CounterViewModel>()
            .BuildServiceProvider();

        window
            .SetTitle("Photino MVVM Counter")
            .RegisterCustomSchemeHandler(new Uri(embeddedAppUrl).Scheme, AppSchemeHandler.Handle)
            .LoadRawString($"""<meta http-equiv="refresh" content="0; URL='{appUrl}'" />""")
            .RegisterWebMessageReceivedHandler((sender, messageJson) =>
            {
                var dispatcher = serviceProvider.GetRequiredService<CommandMessageDispatcher>();
                dispatcher.Dispatch(messageJson);
            });

        window.WaitForClose();
    }
}
