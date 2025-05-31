import { createNanoEvents } from 'nanoevents'
import {
  AppCommand,
  AppEvent,
  CommandMessage,
  CommandPayload,
  EventMessage,
  EventPayload,
  ViewId,
  ViewModelTypeName
} from '../types/apiTypes'
import { ViewModelError } from '../types/viewModelError'

export type EventEmitterKey = `${ViewId}:${string}`

const eventEmitter = createNanoEvents<Record<EventEmitterKey, (payload: unknown) => void>>()

export const initializeEventHandler = () => {
  window.external.receiveMessage((json) => {
    const { viewId, event, payload } = JSON.parse(json) as EventMessage<EventPayload>
    eventEmitter.emit(`${viewId}:${event}`, payload)
  })
}

export const createEventSubscriber = <TEvent extends EventPayload>(viewId: ViewId) => {
  const subscribeEvent = <TName extends TEvent['event']>(
    viewId: ViewId,
    eventName: TName,
    callback: (payload: Extract<TEvent, { event: TName }>['payload']) => void
  ) => {
    const key = `${viewId}:${eventName}` as EventEmitterKey
    return eventEmitter.on(key, (payload) =>
      callback(payload as Extract<TEvent, { event: TName }>['payload'])
    )
  }

  return <TName extends TEvent['event']>(
    eventName: TName,
    callback: (payload: Extract<TEvent, { event: TName }>['payload']) => void
  ) => subscribeEvent<TName>(viewId, eventName, callback)
}

export const createCommandDispatcher = <TCommand extends CommandPayload>(viewId: ViewId) => {
  const dispatchCommand = <TName extends TCommand['command']>(
    viewId: ViewId,
    commandName: TName,
    payload: Extract<TCommand, { command: TName }>['payload']
  ) => {
    const message: CommandMessage<CommandPayload> = { viewId, command: commandName, payload }
    window.external.sendMessage(JSON.stringify(message))
  }

  return <TName extends TCommand['command']>(
    commandName: TName,
    // TCommand の中から、commandName に対応する payload の型を推論して引数にする
    ...[payload]: Extract<TCommand, { command: TName }> extends { payload: infer P }
      ? [payload: P]
      : []
  ) => dispatchCommand<TName>(viewId, commandName, payload)
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
