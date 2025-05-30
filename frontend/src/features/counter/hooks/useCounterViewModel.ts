import useViewModel from '@/lib/api/hooks/useViewModel'
import { ViewId } from '@/lib/api/types/api'
import { useEffect, useState } from 'react'
import { CounterCommand, CounterEvent, CounterState } from '../types/api'

export const useCounterViewModel = (viewId?: ViewId) => {
  const viewModel = useViewModel<CounterEvent, CounterCommand>('Counter', viewId)
  const [state, setState] = useState<CounterState>()
  useEffect(() => viewModel.onEvent('state', setState), [viewModel])
  return { state, ...viewModel }
}
