import useViewModel from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue, useSetAtom } from 'jotai'
import { useEffect } from 'react'
import { CounterCommandEnvelope, CounterEventEnvelope } from '../types/counterTypes'

export const counterViewModelAtom = atom<CounterViewModel>()

const useCounterViewModelInternal = () => {
  const viewModel = useViewModel<CounterEventEnvelope, CounterCommandEnvelope>('Counter')
  const state = viewModel.useViewState('state')
  return { state, ...viewModel }
}

export type CounterViewModel = ReturnType<typeof useCounterViewModelInternal>

export const useCounterViewModel = () => useAtomValue(counterViewModelAtom)

export const useProvideCounterViewModel = () => {
  const viewModel = useCounterViewModelInternal()
  const setViewModelValue = useSetAtom(counterViewModelAtom)

  useEffect(() => {
    setViewModelValue(viewModel)
  }, [viewModel, setViewModelValue])
}
