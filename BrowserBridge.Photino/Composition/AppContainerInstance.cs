using StrongInject;

namespace BrowserBridge.Photino;

public class AppContainerInstance
{
    private object? _container;

    public void Inject(object container) => _container = container;

    public IOwned<T> Resolve<T>() where T : class
    {
        if (_container == null)
            throw new InvalidOperationException("AppContainer is not injected");

        return ((IContainer<T>)_container).Resolve();
    }
}
