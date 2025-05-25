using Microsoft.Extensions.Logging;
using Photino.NET;
using StrongInject;
using TryPhotinoMVVM.Models;
using TryPhotinoMVVM.Services;
using TryPhotinoMVVM.ViewModels;
using TryPhotinoMVVM.Views;

[Register(typeof(PhotinoWindowInstance))]
[Register(typeof(AppService))]
[Register(typeof(EventDispatcher))]
[RegisterFactory(typeof(CommandDispatcherFactory))]
[Register(typeof(ErrorHandlerService))]
[Register(typeof(CounterModel))]
[Register(typeof(CounterViewModel))]
[RegisterFactory(typeof(LoggerFactory<CounterViewModel>))]
[RegisterFactory(typeof(LoggerFactory<Program>))]
public partial class AppContainer : IContainer<AppService>;

public class CommandDispatcherFactory(CounterViewModel vm) : IFactory<CommandDispatcher>
{
    public CommandDispatcher Create() => new CommandDispatcher().Register(vm);
}

public class LoggerFactory<T> : IFactory<ILogger<T>>
{
    public ILogger<T> Create() => new MinimalLogger<T>(new ConsoleLogWriter());
}
