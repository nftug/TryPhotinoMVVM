using System.Text.Json;
using Microsoft.Extensions.Logging;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.ViewModels;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.Views;

public class CommandDispatcher(ILogger<CommandDispatcher> logger)
{
    private readonly Dictionary<Guid, IViewModel> _handlerMap = [];
    private AppContainer? _container;

    public void InjectContainer(AppContainer container) => _container = container;

    public async ValueTask DispatchAsync(string json)
    {
        var message = JsonSerializer.Deserialize(json, JsonContext.Default.CommandMessage);
        if (message == null) return;

        if (Enum.TryParse<DefaultActionType>(message.Command, true, out var action))
        {
            HandleDefaultAction(message, action);
        }
        else if (_handlerMap.TryGetValue(message.ViewId, out var viewModel))
        {
            await viewModel.HandleAsync(message.Command, message.Payload);
        }
    }

    private void HandleDefaultAction(CommandMessage message, DefaultActionType action)
    {
        if (action == DefaultActionType.Init)
        {
            var payload = message.Payload?.ParsePayload(JsonContext.Default.InitCommandPayload)
                ?? throw new InvalidOperationException();

            RegisterAndInit(payload.Type, message.ViewId);
        }
        else if (action == DefaultActionType.Leave)
        {
            if (_handlerMap.TryGetValue(message.ViewId, out var viewModel))
            {
                viewModel.Dispose();
                _handlerMap.Remove(message.ViewId);

                logger.LogInformation("Unregistered a view: (ViewId {ViewId})", message.ViewId);
            }
        }

        void RegisterAndInit(ViewModelType type, Guid viewId)
        {
            if (_container is not { } container) throw new Exception("Container not injected");

            if (!_handlerMap.ContainsKey(viewId))
            {
                _handlerMap[viewId] = type switch
                {
                    ViewModelType.Counter => container.ResolveViewModel<CounterViewModel>(viewId),
                    _ => throw new InvalidOperationException()
                };

                logger.LogInformation("Registered a view for {Type} (ViewId {ViewId})", type, viewId);
            }

            _handlerMap[viewId].HandleInit();
        }
    }
}
