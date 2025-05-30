using Reactive.Bindings.Disposables;

namespace TryPhotinoMVVM.Models.Abstractions;

public abstract class DisposableBase : IDisposable
{
    protected readonly CompositeDisposable Disposable = new();

    protected bool disposedValue;

    protected virtual void Dispose(bool disposing) { }

    public void Dispose()
    {
        Disposable.Dispose();

        Dispose(disposing: true);
        disposedValue = true;
        GC.SuppressFinalize(this);
    }
}
