namespace Reactive.Bindings.TinyLinq;

public static class Observable
{
    public static IObservable<long> Interval(TimeSpan interval)
    {
        return new TimerObservable(interval);
    }

    private class TimerObservable : IObservable<long>, IDisposable
    {
        private readonly TimeSpan _interval;
        private readonly List<IObserver<long>> _observers = new();
        private Timer? _timer;
        private long _count = 0;
        private readonly object _lock = new();

        public TimerObservable(TimeSpan interval)
        {
            _interval = interval;
            _timer = new Timer(_ =>
            {
                long next = Interlocked.Increment(ref _count) - 1;
                lock (_lock)
                {
                    foreach (var observer in _observers.ToArray())
                    {
                        observer.OnNext(next);
                    }
                }
            }, null, _interval, _interval);
        }

        public IDisposable Subscribe(IObserver<long> observer)
        {
            lock (_lock)
            {
                _observers.Add(observer);
            }

            return new Subscription(this, observer);
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _observers.Clear();
                _timer?.Dispose();
                _timer = null;
            }
        }

        private class Subscription : IDisposable
        {
            private readonly TimerObservable _parent;
            private readonly IObserver<long> _observer;
            private bool _disposed;

            public Subscription(TimerObservable parent, IObserver<long> observer)
            {
                _parent = parent;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_disposed) return;
                _disposed = true;

                lock (_parent._lock)
                {
                    _parent._observers.Remove(_observer);
                }
            }
        }
    }
}
