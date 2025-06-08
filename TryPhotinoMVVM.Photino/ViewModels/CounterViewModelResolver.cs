using BrowserBridge;
using BrowserBridge.Photino;
using TryPhotinoMVVM.Core.ViewModels;

namespace TryPhotinoMVVM.Photino.ViewModels;

public class CounterViewModelResolver(AppContainerInstance container) : IViewModelResolver
{
    public string Type => "Counter";

    public IOwnedViewModel Resolve()
    {
        var owned = container.Resolve<CounterViewModel>();
        return new StrongInjectOwnedViewModel(owned);
    }
}
