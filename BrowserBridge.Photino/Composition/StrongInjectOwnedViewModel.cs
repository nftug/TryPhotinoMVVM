using StrongInject;

namespace BrowserBridge.Photino;

public class StrongInjectOwnedViewModel(IOwned<IViewModel> owned) : IOwnedViewModel
{
    public IViewModel Value => owned.Value;

    public void Dispose() => owned.Dispose();
}
