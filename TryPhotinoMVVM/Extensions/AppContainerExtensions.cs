using StrongInject;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.Extensions;

public static class AppContainerExtensions
{
    public static IOwned<T> ResolveViewModel<T>(this AppContainer container, Guid viewId)
        where T : IViewModel
    {
        var owned = ((IContainer<T>)container).Resolve<T>();
        owned.Value.SetViewId(viewId);
        return owned;
    }
}
