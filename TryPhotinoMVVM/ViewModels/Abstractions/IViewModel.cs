using System.Text.Json;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public interface IViewModel : IDisposable
{
    ValueTask HandleAsync(string command, JsonElement? payload);
    void SetViewId(Guid viewId);
    void HandleInit();
}
