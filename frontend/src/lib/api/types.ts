export type ViewModelMessage<T> = {
  type: ViewModelTypeName
  payload?: T
}

export type CommandMessage<T> = {
  type: ViewModelTypeName
  payload?: CommandPayload<T>
}

export type CommandPayload<T> = {
  type: string
  payload?: T
}

export type ViewModelTypeName = string & { __brand: 'ViewModelTypeName' }
