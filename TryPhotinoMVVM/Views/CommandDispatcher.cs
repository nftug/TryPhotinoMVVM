using System.Text.Json;
using Microsoft.Extensions.Logging;
using StrongInject;
using TryPhotinoMVVM.Constants;
using TryPhotinoMVVM.Exceptions;
using TryPhotinoMVVM.Extensions;
using TryPhotinoMVVM.Messages;
using TryPhotinoMVVM.Services;
using TryPhotinoMVVM.ViewModels.Abstractions;

namespace TryPhotinoMVVM.Views;

public class CommandDispatcher(
    AppContainerInstance container, ILogger<CommandDispatcher> logger, ErrorHandlerService errorHandler)
{
    private readonly Dictionary<ViewModelType, Func<Guid, IOwned<IViewModel>>> _viewModelResolvers = [];
    private readonly Dictionary<Guid, IOwned<IViewModel>> _handlerMap = [];

    public CommandDispatcher Register<T>(ViewModelType type)
         where T : class, IViewModel
    {
        _viewModelResolvers[type] = (viewId) =>
        {
            var owned = container.Resolve<T>();
            owned.Value.SetViewId(viewId);
            return owned;
        };
        return this;
    }

    public async ValueTask DispatchAsync(string json)
    {
        var message = JsonSerializer.Deserialize(json, JsonContext.Default.CommandMessage);
        if (message == null) return;

        try
        {
            if (Enum.TryParse<AppActionType>(message.Command, true, out var action))
            {
                HandleDefaultAction(message, action);
            }
            else if (_handlerMap.TryGetValue(message.ViewId, out var viewModel))
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
            var type = message.Payload?.ParsePayload(JsonContext.Default.InitCommandPayload)?.Type
                ?? throw new InvalidOperationException();

            if (!_handlerMap.ContainsKey(message.ViewId))
            {
                if (!_viewModelResolvers.TryGetValue(type, out var viewModelResolver))
                    throw new InvalidOperationException($"ViewModel for {type} is not registered");

                _handlerMap[message.ViewId] = viewModelResolver(message.ViewId);

                logger.LogInformation("Registered a view for {Type}: ViewId {ViewId}", type, message.ViewId);
            }

            _handlerMap[message.ViewId].Value.OnFirstRender();
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
}
