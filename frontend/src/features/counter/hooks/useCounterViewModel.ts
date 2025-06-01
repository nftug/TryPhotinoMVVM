import useViewModel from '@/lib/api/hooks/useViewModel'
import { ViewId } from '@/lib/api/types/apiTypes'
import { CounterCommandEnvelope, CounterEventEnvelope } from '../types/counterTypes'

export const useCounterViewModel = (viewId?: ViewId) => {
  const viewModel = useViewModel<CounterEventEnvelope, CounterCommandEnvelope>('Counter', viewId)
  const state = viewModel.useViewState('state')
  return { state, ...viewModel }
}
