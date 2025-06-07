using BrowserBridge;
using BrowserBridge.Photino;
using StrongInject;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.Presentation;
using TryPhotinoMVVM.ViewModels;

namespace TryPhotinoMVVM.Composition;

#region Modules
[RegisterModule(typeof(PhotinoContainerModule))]
[RegisterModule(typeof(MinimalLoggerModule))]
[Register(typeof(AppService), Scope.SingleInstance)]
[RegisterFactory(typeof(ViewModelResolverFactory), Scope.SingleInstance)]
public class AppBaseModule;

[Register(typeof(CounterModel))]
[Register(typeof(CounterViewModel))]
public class CounterModule;
#endregion

#region Container
[RegisterModule(typeof(AppBaseModule))]
[RegisterModule(typeof(CounterModule))]
public partial class AppContainer : IContainer<AppService>, IViewModelContainer;
#endregion

#region ViewModel Container
public interface IViewModelContainer
    : IViewModelContainerBase, IContainer<CounterViewModel>;
#endregion

#region Factories
public class ViewModelResolverFactory(AppContainerInstance container) : ViewModelResolverFactoryBase(container)
{
    protected override IViewModelResolver RegisterViewModels(IViewModelResolver factory)
        => factory.Register<CounterViewModel>("Counter");
}
#endregion
