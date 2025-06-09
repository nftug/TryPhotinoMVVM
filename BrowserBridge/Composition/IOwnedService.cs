using Microsoft.Extensions.DependencyInjection;
using StrongInject;

namespace BrowserBridge;

public interface IOwnedService<out T> : IDisposable
    where T : class
{
    T Value { get; }
}

public class StrongInjectOwnedService<T>(IOwned<T> owned) : IOwnedService<T>
    where T : class
{
    public T Value => owned.Value;

    public void Dispose() => owned.Dispose();
}

public class MsDependencyInjectionOwnedService<T> : IOwnedService<T>
    where T : class
{
    private readonly IServiceScope _scope;

    public T Value { get; }

    public MsDependencyInjectionOwnedService(IServiceScope scope)
    {
        _scope = scope;
        Value = scope.ServiceProvider.GetRequiredService<T>();
    }

    public void Dispose() => _scope.Dispose();
}