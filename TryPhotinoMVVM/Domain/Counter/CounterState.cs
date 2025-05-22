using TryPhotinoMVVM.Dtos;

namespace TryPhotinoMVVM.Domain.Counter;

public record CounterState(long Count, long? TwiceCount, bool IsProcessing)
{
    public bool CanDecrement => Count > 0;

    public CounterStateDto ToDto()
        => new CounterStateDto(Count, TwiceCount, IsProcessing, CanDecrement);
}
