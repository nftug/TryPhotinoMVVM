using Microsoft.Extensions.Logging;
using StrongInject;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.Services;
using TryPhotinoMVVM.ViewModels;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM;

#region Modules
[Register(typeof(ConsoleLogWriter), typeof(ILogWriter))]
[Register(typeof(MinimalLogger<>), typeof(ILogger<>))]
public class ConsoleLoggerModule;

[Register(typeof(PhotinoWindowInstance), Scope.SingleInstance)]
[Register(typeof(AppService), Scope.SingleInstance)]
[Register(typeof(EventDispatcher), Scope.SingleInstance)]
[RegisterFactory(typeof(CommandDispatcherFactory), Scope.SingleInstance)]
[Register(typeof(ErrorHandlerService), Scope.SingleInstance)]
[Register(typeof(WindowViewModel), Scope.SingleInstance)]
public class AppBaseModule;

[Register(typeof(CounterModel))]
[Register(typeof(CounterViewModel))]
public class CounterModule;
#endregion

#region Container
[RegisterModule(typeof(ConsoleLoggerModule))]
[RegisterModule(typeof(AppBaseModule))]
[RegisterModule(typeof(CounterModule))]
public partial class AppContainer : IContainer<AppService>, IViewModelContainer;
#endregion

#region ViewModel Container
public interface IViewModelContainer
    : IContainer<CounterViewModel>, IContainer<WindowViewModel>;
#endregion

#region Factories
public class CommandDispatcherFactory(
    ILogger<CommandDispatcher> logger, ErrorHandlerService errorHandler) : IFactory<CommandDispatcher>
{
    public CommandDispatcher Create() =>
        new CommandDispatcher(logger, errorHandler)
            .Register<CounterViewModel>(ViewModelType.Counter)
            .Register<WindowViewModel>(ViewModelType.Window);
}
#endregion
