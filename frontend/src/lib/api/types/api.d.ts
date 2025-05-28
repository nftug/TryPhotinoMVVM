export type EventMessage<T extends EventPayload> = {
  type: ViewModelTypeName
} & T

export type EventPayload = {
  event: string
  payload?: unknown
}

export type CommandMessage<T extends CommandPayload> = {
  type: ViewModelTypeName
} & T

export type CommandPayload = {
  command: string
  payload?: unknown
}

export type ViewModelTypeName = 'Counter' | 'Error'
