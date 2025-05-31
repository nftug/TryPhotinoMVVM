import useViewModel from '@/lib/api/hooks/useViewModel'
import { ViewId } from '@/lib/api/types/apiTypes'
import { CounterCommand, CounterEvent } from '../types/api'

export const useCounterViewModel = (viewId?: ViewId) => {
  const viewModel = useViewModel<CounterEvent, CounterCommand>('Counter', viewId)
  const state = viewModel.useViewState('state')
  return { state, ...viewModel }
}
