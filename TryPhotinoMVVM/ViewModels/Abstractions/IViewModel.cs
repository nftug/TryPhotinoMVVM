using System.Text.Json;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public interface IViewModel : IDisposable
{
    ValueTask HandleAsync(CommandMessage message);
    void SetViewId(Guid viewId);
}
