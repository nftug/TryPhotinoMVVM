export type CounterCommandEnvelope =
  | { command: 'increment' }
  | { command: 'decrement' }
  | { command: 'set'; payload: { value: number } }

export type CounterEventEnvelope =
  | { event: 'state'; payload: CounterState }
  | { event: 'fizzBuzz'; payload: CounterFizzBuzz }

export type CounterState = {
  count: number
  twiceCount?: number
  isProcessing: boolean
  canDecrement: boolean
}

export type CounterFizzBuzz = {
  result: 'Fizz' | 'Buzz' | 'FizzBuzz'
}
