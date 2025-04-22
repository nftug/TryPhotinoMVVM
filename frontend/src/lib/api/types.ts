export type OutgoingMessage<T> = {
  type: ViewModelType
  payload?: OutgoingSubMessage<T>
}

export type OutgoingSubMessage<T> = {
  type: string | 'init'
  payload?: T
}

export type IncomingMessage<T> = {
  type: ViewModelType
  payload?: T
}

// ViewModel types
export type CounterInPayload = { value: number }

export type CounterOutPayload = { type: 'increment' | 'decrement' }

// Variation
export type ViewModelType = 'Counter'

export type OutPayloadType = CounterInPayload

export type InPayloadType = CounterInPayload
