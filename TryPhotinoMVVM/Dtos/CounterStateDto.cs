namespace TryPhotinoMVVM.Dtos;

public record CounterStateDto(long Count, long? TwiceCount, bool IsProcessing, bool CanDecrement);
