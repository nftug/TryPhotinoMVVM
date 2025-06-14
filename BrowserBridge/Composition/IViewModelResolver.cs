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

    public string Type => typeof(TViewModel).Name;

    public virtual IOwnedService<IViewModel> Resolve()
        => (IOwnedService<IViewModel>)Container.Resolve<TViewModel>();
}
