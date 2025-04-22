using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Utils;
using TryPhotinoMVVM.ViewModels;

string appUrl = EnvironmentConstants.IsDebugMode ? "http://localhost:5173" : "app://";

var window = new PhotinoWindow()
    .SetTitle("Photino MVVM Counter")
    .RegisterCustomSchemeHandler("app", AppSchemeHandler.Handle);

var services = new ServiceCollection();

// Register application services
services.AddSingleton(window);
services.AddSingleton<OutgoingMessageDispatcher>();
services.AddSingleton<CounterViewModel>();
services.AddSingleton<IncomingMessageDispatcher>(sp =>
{
    var dispatcher = new IncomingMessageDispatcher();
    dispatcher.Register(sp.GetRequiredService<CounterViewModel>());
    return dispatcher;
});

var provider = services.BuildServiceProvider();

window
    .LoadRawString($"""
    <!DOCTYPE html>
    <html>
    <head>
        <meta http-equiv="refresh" content="0; URL='{appUrl}'" />
    </head>
    <body>
        <p>Redirecting to {appUrl}...</p>
    </body>
    </html>
""")
    .RegisterWebMessageReceivedHandler((sender, messageJson) =>
    {
        var dispatcher = provider.GetRequiredService<IncomingMessageDispatcher>();
        dispatcher.Dispatch(messageJson);
    });

window.WaitForClose();
