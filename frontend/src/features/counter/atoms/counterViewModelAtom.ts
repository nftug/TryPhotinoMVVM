import useViewModel, { useProvideViewModel } from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue } from 'jotai'
import { CounterCommandEnvelope, CounterEventEnvelope } from '../types/counterTypes'

export const counterViewModelAtom = atom<CounterViewModel>()

const useCounterViewModelInternal = () => {
  const viewModel = useViewModel<CounterEventEnvelope, CounterCommandEnvelope>('CounterViewModel')
  const state = viewModel.useViewState('state')
  return { state, ...viewModel }
}

export type CounterViewModel = ReturnType<typeof useCounterViewModelInternal>

export const useCounterViewModel = () => useAtomValue(counterViewModelAtom)

export const useProvideCounterViewModel = () =>
  useProvideViewModel(useCounterViewModelInternal, counterViewModelAtom)
