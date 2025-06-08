using TryPhotinoMVVM.Photino.Composition;

namespace TryPhotinoMVVM.Photino;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var container = new AppContainer();
        container.Run<AppService>(app => app.Run(container));
    }
}
