import { ViewModelTypeName } from '@/lib/api/types'
import useViewModel from '@/lib/api/useViewModel'

export const useCounterViewModel = () =>
  useViewModel<CounterViewModel, CounterCommand, CounterEvent>('Counter' as ViewModelTypeName)

export type CounterViewModel = {
  count: number
  twiceCount?: number
  isProcessing: boolean
  canDecrement: boolean
}

export type CounterCommand =
  | { type: 'increment' }
  | { type: 'decrement' }
  | { type: 'set'; payload: { value: number } }

export type CounterEvent = { type: 'fizzBuzz'; payload: 'Fizz' | 'Buzz' | 'FizzBuzz' }
