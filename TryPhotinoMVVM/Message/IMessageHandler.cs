using System.Text.Json;

namespace TryPhotinoMVVM.Message;

public interface IMessageHandler
{
    bool CanHandle(ViewModelType type);
    void Handle(IncomingSubMessage? payload);
}
