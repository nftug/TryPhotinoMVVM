import { createNanoEvents } from 'nanoevents'
import {
  AppCommand,
  AppEvent,
  CommandId,
  CommandMessage,
  CommandPayload,
  EventMessage,
  EventPayload,
  ViewId,
  ViewModelTypeName
} from '../types/apiTypes'
import { ViewModelError } from '../types/viewModelError'

type EventEmitterKey = `${ViewId}:${string}` | `${ViewId}:${string}:${CommandId}`

type CommandPayloadType<TCommand extends CommandPayload, TName extends TCommand['command']> =
  Extract<TCommand, { command: TName }> extends { payload: infer P } ? [payload: P] : []

const eventEmitter = createNanoEvents<Record<EventEmitterKey, (payload: unknown) => void>>()

const generateEmitterKey = (viewId: ViewId, event: string, commandId?: CommandId) =>
  (`${viewId}:${event}` + (commandId ? `:${commandId}` : '')) as EventEmitterKey

export const initializeEventHandler = () => {
  window.external.receiveMessage((json) => {
    const { viewId, commandId, event, payload } = JSON.parse(json) as EventMessage<EventPayload>
    eventEmitter.emit(generateEmitterKey(viewId, event, commandId), payload)
  })
}

export const createEventSubscriber = <TEvent extends EventPayload>(viewId: ViewId) => {
  return <TName extends TEvent['event']>(
    eventName: TName,
    callback: (payload: Extract<TEvent, { event: TName }>['payload']) => void
  ) => {
    const key = generateEmitterKey(viewId, eventName)
    return eventEmitter.on(key, (payload) =>
      callback(payload as Extract<TEvent, { event: TName }>['payload'])
    )
  }
}

export const createCommandDispatcher = <TCommand extends CommandPayload>(viewId: ViewId) => {
  return <TName extends TCommand['command']>(
    commandName: TName,
    ...args: CommandPayloadType<TCommand, TName>
  ) => {
    const payload = args[0]
    const message: CommandMessage<CommandPayload> =
      payload === undefined
        ? { viewId, command: commandName }
        : { viewId, command: commandName, payload }
    window.external.sendMessage(JSON.stringify(message))
  }
}

export const createCommandInvoker = <TEvent extends EventPayload, TCommand extends CommandPayload>(
  viewId: ViewId
) => {
  return <
    TName extends TCommand['command'],
    TMatchedEvent extends Extract<TEvent, { event: `receive:${TName}` }>
  >(
    commandName: TName,
    ...args: CommandPayloadType<TCommand, TName>
  ): Promise<TMatchedEvent['payload']> => {
    const commandId = crypto.randomUUID() as CommandId
    const payload = args[0]
    const message: CommandMessage<CommandPayload> = {
      viewId,
      command: commandName,
      payload,
      commandId
    }

    return new Promise((resolve) => {
      const key = generateEmitterKey(viewId, `receive:${commandName}`, commandId)
      const unsubscribe = eventEmitter.on(key, (payload) => {
        unsubscribe()
        resolve(payload as TMatchedEvent['payload'])
      })
      window.external.sendMessage(JSON.stringify(message))
    })
  }
}

type InitViewOptions = {
  viewId: ViewId
  viewType: ViewModelTypeName
  persist?: boolean
}

export const initView = ({ viewId, viewType, persist }: InitViewOptions) => {
  const dispatch = createCommandDispatcher<AppCommand>(viewId)
  const subscribe = createEventSubscriber<AppEvent>(viewId)

  dispatch('init', { type: viewType })

  const disposeErrorLogger = subscribe('error', (payload) => {
    const error = new ViewModelError(viewId, payload)
    console.error(error)
    console.error(error.details)
  })

  return () => {
    if (!persist) dispatch('leave')
    disposeErrorLogger()
  }
}
