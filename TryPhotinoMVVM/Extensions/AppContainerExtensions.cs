using StrongInject;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.Extensions;

public static class AppContainerExtensions
{
    public static IViewModel ResolveViewModel<T>(this AppContainer container, Guid viewId)
        where T : IViewModel => ((IContainer<T>)container).Resolve<T>().Value.SetViewId(viewId);
}
