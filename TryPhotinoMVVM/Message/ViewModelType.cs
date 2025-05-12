using System.Text.Json.Serialization;

namespace TryPhotinoMVVM.Message;

[JsonConverter(typeof(JsonStringEnumConverter<ViewModelType>))]
public enum ViewModelType
{
    Error,
    Counter
}
