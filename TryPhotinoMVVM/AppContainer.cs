using Microsoft.Extensions.Logging;
using StrongInject;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.Services;
using TryPhotinoMVVM.ViewModels;
using TryPhotinoMVVM.Views;

namespace TryPhotinoMVVM;

[Register(typeof(ConsoleLogWriter), typeof(ILogWriter))]
[Register(typeof(MinimalLogger<>), typeof(ILogger<>))]
public class ConsoleLoggerModule;

[Register(typeof(PhotinoWindowInstance), Scope.SingleInstance)]
[Register(typeof(AppService), Scope.SingleInstance)]
[Register(typeof(EventDispatcher), Scope.SingleInstance)]
[Register(typeof(CommandDispatcher), Scope.SingleInstance)]
[Register(typeof(ErrorHandlerService), Scope.SingleInstance)]
public class AppBaseModule;

[Register(typeof(CounterModel))]
[Register(typeof(CounterViewModel))]
public class CounterModule;

[RegisterModule(typeof(ConsoleLoggerModule))]
[RegisterModule(typeof(AppBaseModule))]
[RegisterModule(typeof(CounterModule))]
public partial class AppContainer
    : IContainer<AppService>, IContainer<CounterViewModel>;
