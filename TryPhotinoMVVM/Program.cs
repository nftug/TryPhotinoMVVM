using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Utils;
using TryPhotinoMVVM.ViewModels;

class Program
{
    [STAThread]
    static void Main()
    {
        string embeddedAppUrl = RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? "app://"
            : "http://app/";
        string appUrl = EnvironmentConstants.IsDebugMode ? "http://localhost:5173/" : embeddedAppUrl;

        var window = new PhotinoWindow();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(window)
            .AddSingleton<OutgoingMessageDispatcher>()
            .AddSingleton<IncomingMessageDispatcher>(sp =>
            {
                var dispatcher = new IncomingMessageDispatcher();
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
                var dispatcher = serviceProvider.GetRequiredService<IncomingMessageDispatcher>();
                dispatcher.Dispatch(messageJson);
            });

        window.WaitForClose();
    }
}
