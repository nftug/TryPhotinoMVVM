using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.NET;
using TryPhotinoMVVM.Message;
using TryPhotinoMVVM.Services;
using TryPhotinoMVVM.ViewModels;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton(new PhotinoWindow())
            .AddSingleton<ViewModelEventDispatcher>()
            .AddSingleton<ErrorHandlerService>()
            .AddSingleton<WindowManagerService>()
            .AddSingleton<CommandMessageDispatcher>(sp =>
                new CommandMessageDispatcher()
                    .Register(sp.GetRequiredService<CounterViewModel>()))
            .AddSingleton<CounterViewModel>()
            .AddLogging(builder => builder.AddConsole())
            .BuildServiceProvider();

        var windowManager = serviceProvider.GetRequiredService<WindowManagerService>();
        windowManager.Run();
    }
}
