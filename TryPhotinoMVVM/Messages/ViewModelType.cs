using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Messages;

[JsonConverter(typeof(JsonStringEnumConverter<ViewModelType>))]
public enum ViewModelType
{
    Window,
    Counter
}
