using System.Text.Json;
using TryPhotinoMVVM.Messages;

namespace TryPhotinoMVVM.ViewModels.Abstractions;

public interface IViewModel
{
    bool CanHandle(ViewModelType type);
    ValueTask HandleAsync(string command, JsonElement? payload);
}
