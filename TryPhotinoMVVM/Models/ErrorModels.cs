using TryPhotinoMVVM.Message;

namespace TryPhotinoMVVM.Models;

public class ViewModelException : Exception
{
    public ViewModelType Type { get; }

    public ViewModelException(ViewModelType type, string? message, Exception? innerException = null)
        : base(message, innerException)
    {
        Type = type;
    }
}

public record ErrorMessage(ViewModelType? Type, string Message);

public record ErrorEvent(ErrorMessage Payload) : EventMessagePayload<ErrorMessage>("error", Payload);
