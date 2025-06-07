using Microsoft.Extensions.Logging;
using StrongInject;
using TryPhotinoMVVM.Composition;
using TryPhotinoMVVM.Enums;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.Presentation;
using TryPhotinoMVVM.Presentation.Dispatchers;
using TryPhotinoMVVM.ViewModels;

namespace TryPhotinoMVVM.Composition;

#region Modules
[Register(typeof(ConsoleLogWriter), typeof(ILogWriter))]
[Register(typeof(MinimalLogger<>), typeof(ILogger<>))]
public class ConsoleLoggerModule;

[Register(typeof(AppContainerInstance), Scope.SingleInstance)]
[Register(typeof(PhotinoWindowInstance), Scope.SingleInstance)]
[Register(typeof(AppService), Scope.SingleInstance)]
[Register(typeof(AppSchemeHandler), Scope.SingleInstance)]
[Register(typeof(EventDispatcher), Scope.SingleInstance)]
[RegisterFactory(typeof(CommandDispatcherFactory), Scope.SingleInstance)]
[Register(typeof(ErrorHandlerService), Scope.SingleInstance)]
[Register(typeof(WindowViewModel))]
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
    AppContainerInstance container, ILogger<CommandDispatcher> logger, ErrorHandlerService errorHandler)
    : IFactory<CommandDispatcher>
{
    public CommandDispatcher Create() =>
        new CommandDispatcher(container, logger, errorHandler)
            .Register<CounterViewModel>(ViewModelType.Counter)
            .Register<WindowViewModel>(ViewModelType.Window);
}
#endregion
