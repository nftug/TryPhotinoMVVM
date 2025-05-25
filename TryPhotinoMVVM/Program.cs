public class Program
{
    [STAThread]
    public static void Main()
    {
        var container = new AppContainer();
        container.Run(app => app.Run());
    }
}
