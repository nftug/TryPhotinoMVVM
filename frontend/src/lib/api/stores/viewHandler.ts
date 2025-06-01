import { camelCase } from 'change-case'
import { createNanoEvents } from 'nanoevents'
import { match, P } from 'ts-pattern'
import {
  CommandEnvelope,
  CommandId,
  CommandMessage,
  EventEnvelope,
  EventMessage,
  ViewId,
  ViewModelTypeName
} from '../types/apiTypes'
import { AppCommandEnvelope, AppEventEnvelope } from '../types/appTypes'
import { ViewModelError } from '../types/viewModelError'

type EmitterKey = `state:${ViewId}:${string}` | `result:${CommandId}:${string}`

type EmitterKeyOptions =
  | { viewId: ViewId; event: string }
  | { commandId: CommandId; commandName: string }

type CommandEnvelopeArguments<
  TCommandEnvelope extends CommandEnvelope,
  TName extends TCommandEnvelope['command']
> = Extract<TCommandEnvelope, { command: TName }> extends { payload: infer P } ? [payload: P] : []

type InitViewOptions = {
  viewId: ViewId
  viewType: ViewModelTypeName
  persist?: boolean
}

const eventEmitter = createNanoEvents<Record<EmitterKey, (payload: unknown) => void>>()

const generateEmitterKey = (opts: EmitterKeyOptions | EventMessage) =>
  match(opts)
    .returnType<EmitterKey>()
    .with(
      { commandName: P.nonNullable, commandId: P.nonNullable },
      ({ commandName, commandId }) => `result:${commandId}:${camelCase(commandName)}`
    )
    .otherwise(({ viewId, event }) => `state:${viewId}:${camelCase(event)}`)

export const initializeEventHandler = () => {
  window.external.receiveMessage((json) => {
    const message = JSON.parse(json) as EventMessage
    eventEmitter.emit(generateEmitterKey(message), message.payload)
  })
}

export const createEventSubscriber = <TEventEnvelope extends EventEnvelope>(viewId: ViewId) => {
  return <TName extends TEventEnvelope['event']>(
    eventName: TName,
    callback: (payload: Extract<TEventEnvelope, { event: TName }>['payload']) => void
  ) => {
    const key = generateEmitterKey({ viewId, event: eventName })
    return eventEmitter.on(key, (payload) =>
      callback(payload as Extract<TEventEnvelope, { event: TName }>['payload'])
    )
  }
}

export const createCommandDispatcher = <TCommandEnvelope extends CommandEnvelope>(
  viewId: ViewId
) => {
  return <TName extends TCommandEnvelope['command']>(
    commandName: TName,
    ...args: CommandEnvelopeArguments<TCommandEnvelope, TName>
  ) => {
    const message: CommandMessage = {
      viewId,
      command: commandName,
      payload: args[0]
    }
    window.external.sendMessage(JSON.stringify(message))
  }
}

export const createCommandInvoker = <
  TEventEnvelope extends EventEnvelope,
  TCommandEnvelope extends CommandEnvelope
>(
  viewId: ViewId
) => {
  type EventWithCommandName = Extract<TEventEnvelope, { commandName: string }>

  return <TName extends TCommandEnvelope['command'] & EventWithCommandName['commandName']>(
    commandName: TName,
    ...args: CommandEnvelopeArguments<TCommandEnvelope, TName>
  ): Promise<Extract<TEventEnvelope, { commandName: TName }>['payload']> => {
    const commandId = crypto.randomUUID() as CommandId
    const message: CommandMessage = {
      viewId,
      command: commandName,
      payload: args[0],
      commandId
    }

    return new Promise<Extract<TEventEnvelope, { commandName: TName }>['payload']>((resolve) => {
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
  const dispatch = createCommandDispatcher<AppCommandEnvelope>(viewId)
  const subscribe = createEventSubscriber<AppEventEnvelope>(viewId)

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
