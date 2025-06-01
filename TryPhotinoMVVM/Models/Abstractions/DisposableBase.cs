using Reactive.Bindings;
using Reactive.Bindings.Disposables;

namespace TryPhotinoMVVM.Models.Abstractions;

public abstract class DisposableBase : IDisposable, INotifiableModel
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

    public virtual void ForceNotify()
    {
        foreach (var item in this.Disposable.OfType<IReactiveProperty>())
        {
            item.ForceNotify();
        }
    }
}
