export type ViewModelMessage<T> = {
  type: ViewModelTypeName
  payload?: T
}

export type CommandMessage<T extends CommandPayload> = {
  type: ViewModelTypeName
  payload?: T
}

export type CommandPayload = {
  type: string
  payload?: unknown
}

export type ViewModelTypeName = string & { __brand: 'ViewModelTypeName' }
