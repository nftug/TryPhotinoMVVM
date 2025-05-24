import useViewModel from '@/lib/api/hooks/useViewModel'
import { useEffect, useState } from 'react'
import { CounterCommand, CounterEvent, CounterState } from '../types/api'

export const useCounterViewModel = () => {
  const viewModel = useViewModel<CounterCommand, CounterEvent>('Counter')
  const [state, setState] = useState<CounterState>()
  useEffect(() => viewModel.onEvent('state', setState), [viewModel])
  return { state, ...viewModel }
}
