using System.Text.Json;
using Microsoft.Extensions.Logging;
using StrongInject;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.Views;

public class CommandDispatcher(ILogger<CommandDispatcher> logger, ErrorHandlerService errorHandler)
{
    private readonly Dictionary<ViewModelType, Func<Guid, AppContainer, IOwned<IViewModel>>> _viewModelResolvers = [];
    private readonly Dictionary<Guid, IOwned<IViewModel>> _handlerMap = [];

    public CommandDispatcher Register<T>(ViewModelType type)
         where T : class, IViewModel
    {
        _viewModelResolvers[type] = (viewId, container) =>
        {
            var owned = ((IContainer<T>)container).Resolve();
            owned.Value.SetViewId(viewId);
            return owned;
        };
        return this;
    }

    public async ValueTask DispatchAsync(string json, AppContainer container)
    {
        var message = JsonSerializer.Deserialize(json, JsonContext.Default.CommandMessage);
        if (message == null) return;

        if (Enum.TryParse<AppActionType>(message.Command, true, out var action))
        {
            HandleDefaultAction(message, action, container);
        }
        else if (_handlerMap.TryGetValue(message.ViewId, out var viewModel))
        {
            await HandleDispatchActionAsync(viewModel.Value, message);
        }
    }

    private void HandleDefaultAction(CommandMessage message, AppActionType action, AppContainer container)
    {
        if (action == AppActionType.Init)
        {
            var type = message.Payload?.ParsePayload(JsonContext.Default.InitCommandPayload)?.Type
                ?? throw new InvalidOperationException();

            if (!_handlerMap.ContainsKey(message.ViewId))
            {
                if (!_viewModelResolvers.TryGetValue(type, out var viewModelResolver))
                    throw new InvalidOperationException($"ViewModel for {type} is not registered");

                _handlerMap[message.ViewId] = viewModelResolver(message.ViewId, container);

                logger.LogInformation("Registered a view for {Type}: ViewId {ViewId}", type, message.ViewId);
            }

            _handlerMap[message.ViewId].Value.HandleInit();
        }
        else if (action == AppActionType.Leave)
        {
            if (_handlerMap.TryGetValue(message.ViewId, out var viewModel))
            {
                viewModel.Dispose();
                _handlerMap.Remove(message.ViewId);

                logger.LogInformation("Unregistered a view: ViewId {ViewId}", message.ViewId);
            }
        }
    }

    private async ValueTask HandleDispatchActionAsync(IViewModel viewModel, CommandMessage message)
    {
        try
        {
            await viewModel.HandleAsync(message);
        }
        catch (Exception e)
        {
            errorHandler.HandleError(new ViewModelException(message.ViewId, e.Message, e));
        }
    }
}
