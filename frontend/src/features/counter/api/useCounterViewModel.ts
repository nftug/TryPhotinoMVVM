import { ViewModelTypeName } from '@/lib/api/types'
import useViewModel from '@/lib/api/useViewModel'

export const useCounterViewModel = () =>
  useViewModel<CounterViewModel, CounterCommand>('Counter' as ViewModelTypeName)

export type CounterViewModel = {
  count: number
  twiceCount?: number
  isProcessing: boolean
}

export type CounterCommand = { type: 'increment' } | { type: 'decrement' }
