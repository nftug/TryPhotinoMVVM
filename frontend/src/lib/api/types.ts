export type ViewModelMessage<T> = {
  type: ViewModelTypeName
  payload?: T
}

export type CommandMessage<T> = {
  type: ViewModelTypeName
  payload?: CommandPayload<T>
}

export type CommandPayload<T> = {
  type: string | 'init'
  payload?: T
}

// ViewModel types
export type CounterViewModel = { count: number }

export type CounterCommand = { type: 'increment' } | { type: 'decrement' }

// Variation
export type ViewModelTypeName = 'Counter'

export type ViewModelType = CounterViewModel

export type CommandType = CounterViewModel
