using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Utils;
using TryPhotinoMVVM.ViewModels;

string appUrl = EnvironmentConstants.IsDebugMode ? "http://localhost:5173" : "app://";

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
    .RegisterCustomSchemeHandler("app", AppSchemeHandler.Handle)
    .LoadRawString($"""<meta http-equiv="refresh" content="0; URL='{appUrl}'" />""")
    .RegisterWebMessageReceivedHandler((sender, messageJson) =>
    {
        var dispatcher = serviceProvider.GetRequiredService<IncomingMessageDispatcher>();
        dispatcher.Dispatch(messageJson);
    });

window.WaitForClose();
