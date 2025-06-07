using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BrowserBridge;

public class CommandDispatcher(
    IViewModelResolver viewModelFactory, ILogger<CommandDispatcher> logger, IErrorHandler errorHandler)
{
    private readonly Dictionary<Guid, IViewModelHandle> _viewModelMap = [];

    public async ValueTask DispatchAsync(string json)
    {
        var message = JsonSerializer.Deserialize(json, BridgeJsonContext.Default.CommandMessage);
        if (message == null) return;

        try
        {
            if (Enum.TryParse<AppActionType>(message.Command, true, out var action))
            {
                HandleDefaultAction(message, action);
            }
            else if (_viewModelMap.TryGetValue(message.ViewId, out var viewModel))
            {
                await viewModel.Value.HandleAsync(message);
            }
        }
        catch (Exception e)
        {
            errorHandler.HandleError(new ViewModelException(message.ViewId, e.Message, e));
        }
    }

    private void HandleDefaultAction(CommandMessage message, AppActionType action)
    {
        if (action == AppActionType.Init)
        {
            var type = message.Payload?.ParsePayload(BridgeJsonContext.Default.InitCommandPayload)?.Type
                ?? throw new InvalidOperationException("Type is required for Init action.");

            if (!_viewModelMap.ContainsKey(message.ViewId))
            {
                var viewModelInstance = viewModelFactory.Resolve(type);
                viewModelInstance.Value.SetViewId(message.ViewId);

                _viewModelMap[message.ViewId] = viewModelInstance;

                logger.LogInformation("Registered a view for {Type}: ViewId {ViewId}", type, message.ViewId);
            }

            _viewModelMap[message.ViewId].Value.OnFirstRender();
        }
        else if (action == AppActionType.Leave)
        {
            if (_viewModelMap.TryGetValue(message.ViewId, out var viewModel))
            {
                viewModel.Dispose();
                _viewModelMap.Remove(message.ViewId);

                logger.LogInformation("Unregistered a view: ViewId {ViewId}", message.ViewId);
            }
        }
    }
}
