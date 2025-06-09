namespace BrowserBridge;

public interface IViewModelResolver
{
    string Type { get; }
    IOwnedService<IViewModel> Resolve();
}

public abstract class ViewModelResolverBase<TViewModel>(IContainerInstance container) : IViewModelResolver
    where TViewModel : class, IViewModel
{
    protected readonly IContainerInstance Container = container;

    public abstract string Type { get; }

    public virtual IOwnedService<IViewModel> Resolve()
        => (IOwnedService<IViewModel>)Container.Resolve<TViewModel>();
}
