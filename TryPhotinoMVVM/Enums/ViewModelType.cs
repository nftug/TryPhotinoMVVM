using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<ViewModelType>))]
public enum ViewModelType
{
    Window,
    Counter
}
