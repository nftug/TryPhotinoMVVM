using BrowserBridge;
using R3;
using TryPhotinoMVVM.Domain.Counter.Entities;
using TryPhotinoMVVM.Domain.Counter.Enums;

namespace TryPhotinoMVVM.Models;

public class CounterModel : DisposableBase
{
    private readonly ReactiveProperty<long> _count = new();
    private readonly ReactiveProperty<long?> _twiceCount = new();
    private readonly ReactiveProperty<bool> _isProcessing = new();

    public ReadOnlyReactiveProperty<CounterState> CounterState { get; }
    public ReactiveCommand<FizzBuzz> FizzBuzzReceivedCommand { get; } = new();

    public CounterModel()
    {
        _count.AddTo(Disposable);
        _twiceCount.AddTo(Disposable);
        _isProcessing.AddTo(Disposable);

        CounterState = _count
            .CombineLatest(_twiceCount, (count, twice) => (count, twice))
            .CombineLatest(_isProcessing, (x, isProcessing) => (x.count, x.twice, isProcessing))
            .Select(x => new CounterState(x.count, x.twice, x.isProcessing))
            .ToReadOnlyReactiveProperty(new(0, null, false))
            .AddTo(Disposable);

        FizzBuzzReceivedCommand.AddTo(Disposable);
    }

    public async Task ChangeCountAsync(long value)
    {
        if (_isProcessing.Value || _count.Value == value) return;
        if (value < 0)
        {
            _count.ForceNotify();
            throw new Exception("Invalid value!");
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
