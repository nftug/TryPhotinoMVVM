import { useContext } from 'react'
import {
  CounterStateViewModelContext,
  CounterViewModelContext,
  GlobalCounterStateViewModelContext,
  GlobalCounterViewModelContext
} from '../contexts/CounterViewModelContexts'

export const useCounterViewModelContext = () => {
  const stateCtx = useContext(CounterStateViewModelContext)
  const vmCtx = useContext(CounterViewModelContext)

  if (!stateCtx || !vmCtx) {
    throw new Error('useCounterViewModelContext must be used within CounterViewModelProvider')
  }

  return { state: stateCtx.state, ...vmCtx }
}

export const useGlobalCounterViewModelContext = () => {
  const stateCtx = useContext(GlobalCounterStateViewModelContext)
  const vmCtx = useContext(GlobalCounterViewModelContext)

  if (!stateCtx || !vmCtx) {
    throw new Error('useCounterViewModelContext must be used within CounterViewModelProvider')
  }

  return { state: stateCtx.state, ...vmCtx }
}

export type UseCounterViewModelContext = typeof useCounterViewModelContext
