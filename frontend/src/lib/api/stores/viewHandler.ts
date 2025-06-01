import { camelCase } from 'change-case'
import { createNanoEvents } from 'nanoevents'
import { match, P } from 'ts-pattern'
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

type EmitterKey = `state:${ViewId}:${string}` | `result:${CommandId}:${string}`

type EmitterKeyOptions =
  | { viewId: ViewId; event: string }
  | { commandId: CommandId; commandName: string }

type CommandPayloadArguments<TCommand extends CommandPayload, TName extends TCommand['command']> =
  Extract<TCommand, { command: TName }> extends { payload: infer P } ? [payload: P] : []

type InitViewOptions = {
  viewId: ViewId
  viewType: ViewModelTypeName
  persist?: boolean
}

const eventEmitter = createNanoEvents<Record<EmitterKey, (payload: unknown) => void>>()

const generateEmitterKey = (opts: EmitterKeyOptions) =>
  match(opts)
    .returnType<EmitterKey>()
    .with(
      { commandName: P.nonNullable, commandId: P.nonNullable },
      ({ commandName, commandId }) => `result:${commandId}:${camelCase(commandName)}`
    )
    .otherwise(({ viewId, event }) => `state:${viewId}:${camelCase(event)}`)

export const initializeEventHandler = () => {
  window.external.receiveMessage((json) => {
    const message = JSON.parse(json) as EventMessage<EventPayload>
    eventEmitter.emit(generateEmitterKey(message), message.payload)
  })
}

export const createEventSubscriber = <TEvent extends EventPayload>(viewId: ViewId) => {
  return <TName extends TEvent['event']>(
    eventName: TName,
    callback: (payload: Extract<TEvent, { event: TName }>['payload']) => void
  ) => {
    const key = generateEmitterKey({ viewId, event: eventName })
    return eventEmitter.on(key, (payload) =>
      callback(payload as Extract<TEvent, { event: TName }>['payload'])
    )
  }
}

export const createCommandDispatcher = <TCommand extends CommandPayload>(viewId: ViewId) => {
  return <TName extends TCommand['command']>(
    commandName: TName,
    ...args: CommandPayloadArguments<TCommand, TName>
  ) => {
    const message: CommandMessage<CommandPayload> = {
      viewId,
      command: commandName,
      payload: args[0]
    }
    window.external.sendMessage(JSON.stringify(message))
  }
}

export const createCommandInvoker = <TEvent extends EventPayload, TCommand extends CommandPayload>(
  viewId: ViewId
) => {
  type EventWithCommandName = Extract<TEvent, { commandName: string }>

  return <TName extends TCommand['command'] & EventWithCommandName['commandName']>(
    commandName: TName,
    ...args: CommandPayloadArguments<TCommand, TName>
  ): Promise<Extract<TEvent, { commandName: TName }>['payload']> => {
    const commandId = crypto.randomUUID() as CommandId
    const message: CommandMessage<CommandPayload> = {
      viewId,
      command: commandName,
      payload: args[0],
      commandId
    }

    return new Promise<Extract<TEvent, { commandName: TName }>['payload']>((resolve) => {
      const key = generateEmitterKey({ commandName, commandId })
      const unsubscribe = eventEmitter.on(key, (payload) => {
        unsubscribe()
        resolve(payload)
      })
      window.external.sendMessage(JSON.stringify(message))
    })
  }
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
