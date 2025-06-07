using TryPhotinoMVVM.Dtos.Counter.Events;

namespace TryPhotinoMVVM.Domain.Counter.Entities;

public class CounterState(long count, long? twiceCount, bool isProcessing)
{
    public long Count { get; } = count;
    public long? TwiceCount { get; } = twiceCount;
    public bool IsProcessing { get; } = isProcessing;

    public bool CanDecrement => Count > 0;

    public CounterStateDto ToDto()
        => new CounterStateDto(Count, TwiceCount, IsProcessing, CanDecrement);
}
