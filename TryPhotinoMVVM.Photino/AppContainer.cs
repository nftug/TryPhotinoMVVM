using BrowserBridge;
using BrowserBridge.Photino;
using StrongInject;
using TryPhotinoMVVM.Core.Models;
using TryPhotinoMVVM.Core.ViewModels;

namespace TryPhotinoMVVM.Photino;

#region Modules
[RegisterModule(typeof(PhotinoContainerModule))]
[RegisterModule(typeof(MinimalLoggerModule))]
[Register(typeof(AppService), Scope.SingleInstance)]
public class AppBaseModule;

[Register(typeof(CounterModel))]
[Register(typeof(CounterViewModel))]
[Register(typeof(ViewModelResolver<CounterViewModel>), typeof(IViewModelResolver))]
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
