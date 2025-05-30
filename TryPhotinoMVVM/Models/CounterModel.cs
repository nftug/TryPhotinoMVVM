using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.TinyLinq;
using TryPhotinoMVVM.Domain.Counter;
using TryPhotinoMVVM.Domain.Enums;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Models.Abstractions;

namespace TryPhotinoMVVM.Models;

public class CounterModel : DisposableBase
{
    private readonly ReactivePropertySlim<long> _count = new();
    private readonly ReactivePropertySlim<long?> _twiceCount = new();
    private readonly ReactivePropertySlim<bool> _isProcessing = new();

    public ReadOnlyReactivePropertySlim<CounterState?> CounterState { get; }
    public ReactiveCommandSlim<FizzBuzz> FizzBuzzReceivedCommand { get; } = new();

    public CounterModel()
    {
        _count.AddTo(Disposable);
        _twiceCount.AddTo(Disposable);
        _isProcessing.AddTo(Disposable);

        CounterState = _count
            .CombineLatest(_twiceCount, (count, twice) => (count, twice))
            .CombineLatest(_isProcessing, (x, isProcessing) => (x.count, x.twice, isProcessing))
            .Select(x => new CounterState(x.count, x.twice, x.isProcessing))
            .ToReadOnlyReactivePropertySlim(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe)
            .AddTo(Disposable);
    }

    public void ForceNotify()
    {
        _count.ForceNotify();
        _twiceCount.ForceNotify();
        _isProcessing.ForceNotify();
    }

    public async Task ChangeCountAsync(long value)
    {
        if (_isProcessing.Value || _count.Value == value) return;
        if (value < 0)
        {
            _count.ForceNotify();
            return;
        }

        _count.Value = value;
        _isProcessing.Value = true;

        var twiceCountTask = GetTwiceCountAsync(value);
        var fizzBuzzTask = GetFizzBuzzAsync(value);
        await Task.WhenAll(twiceCountTask, fizzBuzzTask);

        _twiceCount.Value = twiceCountTask.Result;
        if (fizzBuzzTask.Result is { } fizzBuzz)
            FizzBuzzReceivedCommand.Execute(fizzBuzz);

        _isProcessing.Value = false;
    }

    private async Task<long> GetTwiceCountAsync(long count)
    {
        await Task.Delay(200);
        return count * 2;
    }

    private async Task<FizzBuzz?> GetFizzBuzzAsync(long count)
    {
        await Task.Delay(500);
        return count switch
        {
            0 => null,
            var x when x % 15 == 0 => FizzBuzz.FizzBuzz,
            var x when x % 5 == 0 => FizzBuzz.Buzz,
            var x when x % 3 == 0 => FizzBuzz.Fizz,
            _ => null,
        };
    }
}
