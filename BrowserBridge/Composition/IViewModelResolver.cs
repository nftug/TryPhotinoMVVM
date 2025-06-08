namespace BrowserBridge;

public interface IViewModelResolver
{
    string Type { get; }
    IOwnedViewModel Resolve();
}

public interface IOwnedViewModel : IDisposable
{
    IViewModel Value { get; }
}
