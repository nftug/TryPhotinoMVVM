export type ViewId = ReturnType<typeof crypto.randomUUID> & { readonly __brand: 'ViewId' }

export type EventMessage<T extends EventPayload> = {
  viewId: ViewId
} & T

export type EventPayload = {
  event: string
  payload?: unknown
}

export type CommandMessage<T extends CommandPayload> = {
  viewId: ViewId
} & T

export type CommandPayload = {
  command: string
  payload?: unknown
}

export type ViewModelTypeName = 'Counter' | 'Error'
