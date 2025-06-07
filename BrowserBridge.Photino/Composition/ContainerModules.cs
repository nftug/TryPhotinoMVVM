using Microsoft.Extensions.Logging;
using StrongInject;

namespace BrowserBridge.Photino;

#region Modules
[Register(typeof(CommandDispatcher), Scope.SingleInstance)]
[Register(typeof(ErrorHandler), typeof(IErrorHandler))]
[Register(typeof(WindowViewModel))]
public class BridgeContainerModule;

[RegisterModule(typeof(BridgeContainerModule))]
[Register(typeof(AppContainerInstance), Scope.SingleInstance)]
[Register(typeof(PhotinoWindowInstance), Scope.SingleInstance)]
[Register(typeof(PhotinoDialogService), typeof(IDialogService))]
[Register(typeof(PhotinoEventDispatcher), Scope.SingleInstance, typeof(IEventDispatcher))]
public class PhotinoContainerModule;

[Register(typeof(ConsoleLogWriter), typeof(ILogWriter))]
[Register(typeof(MinimalLogger<>), typeof(ILogger<>))]
public class MinimalLoggerModule;
#endregion

#region Container Interfaces
public interface IViewModelContainerBase : IContainer<WindowViewModel>;
#endregion

#region Factories
public abstract class ViewModelResolverFactoryBase(AppContainerInstance container) : IFactory<IViewModelResolver>
{
    public IViewModelResolver Create()
    {
        var factory = new StrongInjectViewModelFactory(container).Register<WindowViewModel>("Window");
        return RegisterViewModels(factory);
    }

    protected abstract IViewModelResolver RegisterViewModels(IViewModelResolver factory);
}
#endregion
