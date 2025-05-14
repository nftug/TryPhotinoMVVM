import { ViewModelTypeName } from '@/lib/api/types'
import useViewModel from '@/lib/api/useViewModel'
import { useEffect } from 'react'

export const useCounterViewModel = () => {
  const { onEvent, ...rest } = useViewModel<CounterViewModel, CounterCommand, CounterEvent>(
    'Counter' as ViewModelTypeName
  )

  useEffect(() => {
    const unsubscribeFizzBuzz = onEvent('fizzBuzz', (payload) => {
      alert(payload)
    })
    return () => unsubscribeFizzBuzz()
  }, [onEvent])

  return { ...rest }
}

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
