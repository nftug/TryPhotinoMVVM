using StrongInject;

namespace TryPhotinoMVVM.Services;

public class AppContainerInstance
{
    private AppContainer? _container;

    public void Inject(AppContainer container) => _container = container;

    public IOwned<T> Resolve<T>() where T : class
    {
        if (_container == null)
            throw new InvalidOperationException("AppContainer is not injected");

        return ((IContainer<T>)_container).Resolve();
    }
}
