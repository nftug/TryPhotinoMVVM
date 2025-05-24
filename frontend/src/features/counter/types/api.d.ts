export type CounterState = {
  count: number
  twiceCount?: number
  isProcessing: boolean
  canDecrement: boolean
}

export type CounterCommand =
  | { type: 'increment' }
  | { type: 'decrement' }
  | { type: 'set'; payload: { value: number } }

export type CounterEvent =
  | { type: 'state'; payload: CounterState }
  | { type: 'fizzBuzz'; payload: 'Fizz' | 'Buzz' | 'FizzBuzz' }
