using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.NET;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.Services;
using TryPhotinoMVVM.ViewModels;
using TryPhotinoMVVM.Views;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton(new PhotinoWindow())
            .AddSingleton<ViewModelEventDispatcher>()
            .AddSingleton<ErrorHandlerService>()
            .AddSingleton<AppWindowManagerService>()
            .AddSingleton<CommandMessageDispatcher>(sp =>
                new CommandMessageDispatcher()
                    .Register(sp.GetRequiredService<CounterViewModel>()))
            .AddSingleton<CounterModel>()
            .AddSingleton<CounterViewModel>()
            .AddLogging(builder => builder.AddConsole())
            .BuildServiceProvider();

        var windowManager = serviceProvider.GetRequiredService<AppWindowManagerService>();
        windowManager.Run();
    }
}
