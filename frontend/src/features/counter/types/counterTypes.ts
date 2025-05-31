export type CounterState = {
  count: number
  twiceCount?: number
  isProcessing: boolean
  canDecrement: boolean
}

export type CounterFizzBuzz = {
  result: 'Fizz' | 'Buzz' | 'FizzBuzz'
}

export type CounterCommand =
  | { command: 'increment' }
  | { command: 'decrement' }
  | { command: 'set'; payload: { value: number } }

export type CounterEvent =
  | { event: 'state'; payload: CounterState }
  | { event: 'fizzBuzz'; payload: CounterFizzBuzz }
