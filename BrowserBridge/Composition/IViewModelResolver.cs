namespace BrowserBridge;

public interface IViewModelResolver
{
    IViewModelResolver Register<T>(string type) where T : class, IViewModel;
    IViewModelHandle Resolve(string type);
}

public interface IViewModelHandle : IDisposable
{
    IViewModel Value { get; }
}
