using StrongInject;

namespace BrowserBridge.Photino;

public class StrongInjectViewModelFactory(AppContainerInstance container) : IViewModelResolver
{
    private readonly Dictionary<string, Func<IOwned<IViewModel>>> _resolvers = [];

    public IViewModelResolver Register<T>(string type) where T : class, IViewModel
    {
        _resolvers[type] = () => container.Resolve<T>();
        return this;
    }

    public IViewModelHandle Resolve(string type)
    {
        if (_resolvers.TryGetValue(type, out var resolver))
        {
            var owned = resolver();
            return new StrongInjectViewModelHandle(owned);
        }

        throw new InvalidOperationException($"No ViewModel registered for type: {type}");
    }
}

public class StrongInjectViewModelHandle(IOwned<IViewModel> owned) : IViewModelHandle
{
    public IViewModel Value => owned.Value;

    public void Dispose() => owned.Dispose();
}
