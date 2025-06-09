using TryPhotinoMVVM.Core.Composition;

namespace TryPhotinoMVVM.Photino;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var container = new AppContainer();
        container.Run<AppService>(app => app.Run(container));

        /*
        var serviceCollection = new ServiceCollection()
            .AddPhotinoBrowserBridge()
            .AddMinimalLogger()
            .AddAppServices()
            .AddSingleton<AppService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        serviceProvider.GetRequiredService<AppService>().Run(serviceProvider);
        */
    }
}
