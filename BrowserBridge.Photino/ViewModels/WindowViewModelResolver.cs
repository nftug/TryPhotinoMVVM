namespace BrowserBridge.Photino;

public class WindowViewModelResolver(AppContainerInstance container) : IViewModelResolver
{
    public string Type => "Window";

    public IOwnedViewModel Resolve()
    {
        var owned = container.Resolve<WindowViewModel>();
        return new StrongInjectOwnedViewModel(owned);
    }
}
