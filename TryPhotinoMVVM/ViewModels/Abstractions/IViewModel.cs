using System.Text.Json;
using TryPhotinoMVVM.Dtos.Abstractions.Commands;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public interface IViewModel : IDisposable
{
    ValueTask HandleAsync(CommandMessage message);
    void SetViewId(Guid viewId);
    void OnFirstRender();
}
