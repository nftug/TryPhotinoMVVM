import { ViewModelErrorEventResult } from './viewModelError'

export type ViewModelTypeName = 'Window' | 'Counter'

export type ViewId = ReturnType<typeof crypto.randomUUID> & { readonly __brand: 'ViewId' }

export type CommandId = ReturnType<typeof crypto.randomUUID> & { readonly __brand: 'CommandId' }

export type EventMessage<T extends EventPayload> = {
  viewId: ViewId
  commandId?: CommandId
} & T

export type EventPayload = {
  event: string
  payload?: unknown
}

export type CommandMessage<T extends CommandPayload> = {
  viewId: ViewId
  commandId?: CommandId
} & T

export type CommandPayload = {
  command: string
  payload?: unknown
}

// Default events and commands
export type AppEvent = {
  event: 'error'
  payload: ViewModelErrorEventResult
}

export type AppCommand =
  | { command: 'init'; payload: { type: ViewModelTypeName } }
  | { command: 'leave' }
