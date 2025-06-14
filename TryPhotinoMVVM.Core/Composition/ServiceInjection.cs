using BrowserBridge;
using Microsoft.Extensions.DependencyInjection;
using StrongInject;
using TryPhotinoMVVM.Core.Models;
using TryPhotinoMVVM.Core.ViewModels;

namespace TryPhotinoMVVM.Core.Composition;

#region StrongInject Modules
[Register(typeof(CounterModel))]
[Register(typeof(CounterViewModel))]
[Register(typeof(ViewModelResolver<CounterViewModel>), typeof(IViewModelResolver))]
public class CounterModule;

public interface IViewModelContainer
    : IViewModelContainerBase, IContainer<CounterViewModel>;
#endregion

#region MsDependencyInjection Modules
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<CounterModel>();
        services.AddScoped<CounterViewModel>();
        services.AddScoped<IViewModelResolver, ViewModelResolver<CounterViewModel>>();

        return services;
    }
}
#endregion
