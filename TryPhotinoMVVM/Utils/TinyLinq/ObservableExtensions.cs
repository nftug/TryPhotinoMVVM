using Reactive.Bindings.Disposables;

namespace Reactive.Bindings.TinyLinq;

public static class ObservableExtensions
{
    public static IDisposable Subscribe<T>(
        this IObservable<T> source,
        Action<T> onNext,
        Action<Exception>? onError = null,
        Action? onCompleted = null
    )
    {
        return source.Subscribe(new Observer<T>(onNext, onError, onCompleted));
    }

    public static IObservable<TResult> CombineLatest<T1, T2, TResult>(
        this IObservable<T1> first,
        IObservable<T2> second,
        Func<T1, T2, TResult> resultSelector)
    {
        return new CombineLatestObservable<T1, T2, TResult>(first, second, resultSelector);
    }

    public static IObservable<(T1, T2)> CombineLatest<T1, T2>(this IObservable<T1> first, IObservable<T2> second)
    {
        return first.CombineLatest(second, (First, Second) => (First, Second));
    }

    public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
    {
        return new AnonymousObservable<T>(observer =>
        {
            T? last = default;
            bool hasValue = false;

            return source.Subscribe(value =>
            {
                if (!hasValue || !EqualityComparer<T>.Default.Equals(last!, value))
                {
                    hasValue = true;
                    last = value;
                    observer.OnNext(value);
                }
            },
            observer.OnError,
            observer.OnCompleted);
        });
    }

    public static IObservable<T> Skip<T>(this IObservable<T> source, int count)
    {
        return new SkipObservable<T>(source, count);
    }

    /*
    private classes
    */
    private class Observer<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        public Observer(Action<T> onNext, Action<Exception>? onError, Action? onCompleted)
        {
            _onNext = onNext;
            _onError = onError ?? (_ => { });
            _onCompleted = onCompleted ?? (() => { });
        }

        public void OnNext(T value) => _onNext(value);
        public void OnError(Exception error) => _onError(error);
        public void OnCompleted() => _onCompleted();
    }

    private class AnonymousObservable<T> : IObservable<T>
    {
        private readonly Func<IObserver<T>, IDisposable> _subscribe;

        public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
        {
            _subscribe = subscribe;
        }

        public IDisposable Subscribe(IObserver<T> observer) => _subscribe(observer);
    }

    private class CombineLatestObservable<T1, T2, TResult> : IObservable<TResult>
    {
        private readonly IObservable<T1> _a;
        private readonly IObservable<T2> _b;
        private readonly Func<T1, T2, TResult> _resultSelector;

        public CombineLatestObservable(IObservable<T1> a, IObservable<T2> b, Func<T1, T2, TResult> resultSelector)
        {
            _a = a;
            _b = b;
            _resultSelector = resultSelector;
        }

        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            T1? latestA = default!;
            T2? latestB = default!;
            bool hasA = false;
            bool hasB = false;
            var gate = new object();

            var subA = _a.Subscribe(x =>
            {
                lock (gate)
                {
                    latestA = x;
                    hasA = true;
                    if (hasB)
                    {
                        observer.OnNext(_resultSelector(latestA!, latestB!));
                    }
                }
            });

            var subB = _b.Subscribe(y =>
            {
                lock (gate)
                {
                    latestB = y;
                    hasB = true;
                    if (hasA)
                    {
                        observer.OnNext(_resultSelector(latestA!, latestB!));
                    }
                }
            });

            return new CompositeDisposable([subA, subB]);
        }
    }

    private class SkipObservable<T> : IObservable<T>
    {
        private readonly IObservable<T> _source;
        private readonly int _count;

        public SkipObservable(IObservable<T> source, int count)
        {
            _source = source;
            _count = count;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            int seen = 0;
            return _source.Subscribe(new SkipObserver(observer, _count, ref seen));
        }

        private class SkipObserver : IObserver<T>
        {
            private readonly IObserver<T> _downstream;
            private readonly int _count;
            private int _seen;

            public SkipObserver(IObserver<T> downstream, int count, ref int seen)
            {
                _downstream = downstream;
                _count = count;
                _seen = seen;
            }

            public void OnNext(T value)
            {
                if (_seen >= _count)
                {
                    _downstream.OnNext(value);
                }
                else
                {
                    _seen++;
                }
            }

            public void OnError(Exception error) => _downstream.OnError(error);
            public void OnCompleted() => _downstream.OnCompleted();
        }
    }
}
