namespace TryPhotinoMVVM.Messages;

public class ViewModelException : Exception
{
    public Guid ViewId { get; }

    public ViewModelException(Guid viewId, string? message, Exception? innerException = null)
        : base(message, innerException)
    {
        ViewId = viewId;
    }
}

public record ViewModelError(string Message);

public record ViewModelErrorEvent(ViewModelError Payload) : EventMessage<ViewModelError>("error", Payload);
