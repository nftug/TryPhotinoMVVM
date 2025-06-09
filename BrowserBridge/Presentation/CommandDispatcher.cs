using System.Text.Json;
using Microsoft.Extensions.Logging;
using StrongInject;

namespace BrowserBridge;

public class CommandDispatcher(
    IEnumerable<IViewModelResolver> viewModelResolvers, ILogger<CommandDispatcher> logger, IErrorHandler errorHandler)
{
    private readonly Dictionary<Guid, IOwnedService<IViewModel>> _viewModelMap = [];

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

            if (_viewModelMap.ContainsKey(message.ViewId))
                throw new InvalidOperationException($"ViewId {message.ViewId} is already registered.");

            var resolver = viewModelResolvers
                .FirstOrDefault(r => r.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException($"No resolver found for type: {type}");

            var viewModelOwned = resolver.Resolve();

            viewModelOwned.Value.SetViewId(message.ViewId);
            _viewModelMap[message.ViewId] = viewModelOwned;

            logger.LogInformation("Registered a view for {Type}: ViewId {ViewId}", type, message.ViewId);
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

public class CommandDispatcherFactory(
    IViewModelResolver[] viewModelResolvers, ILogger<CommandDispatcher> logger, IErrorHandler errorHandler)
    : IFactory<CommandDispatcher>
{
    public CommandDispatcher Create() =>
        new CommandDispatcher(viewModelResolvers, logger, errorHandler);
}
