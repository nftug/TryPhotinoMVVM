import useViewModel from '@/lib/api/hooks/useViewModel'
import {
  CounterStateViewModelContext,
  CounterViewModelContext,
  GlobalCounterStateViewModelContext,
  GlobalCounterViewModelContext
} from '../contexts/CounterViewModelContexts'
import { CounterCommandEnvelope, CounterEventEnvelope } from '../types/counterTypes'

const useCounterViewModel = () => {
  const viewModel = useViewModel<CounterEventEnvelope, CounterCommandEnvelope>('Counter')
  const state = viewModel.useViewState('state')
  return { state, ...viewModel }
}

export const CounterViewModelProvider = ({ children }: { children?: React.ReactNode }) => {
  const { state, ...viewModel } = useCounterViewModel()

  return (
    <CounterStateViewModelContext.Provider value={{ state }}>
      <CounterViewModelContext.Provider value={viewModel}>
        {children}
      </CounterViewModelContext.Provider>
    </CounterStateViewModelContext.Provider>
  )
}

export const GlobalCounterViewModelProvider = ({ children }: { children?: React.ReactNode }) => {
  const { state, ...viewModel } = useCounterViewModel()

  return (
    <GlobalCounterStateViewModelContext.Provider value={{ state }}>
      <GlobalCounterViewModelContext.Provider value={viewModel}>
        {children}
      </GlobalCounterViewModelContext.Provider>
    </GlobalCounterStateViewModelContext.Provider>
  )
}
