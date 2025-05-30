using TryPhotinoMVVM.Services;

namespace TryPhotinoMVVM;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var container = new AppContainer();
        container.Run<AppService>(app => app.Run(container));
    }
}
