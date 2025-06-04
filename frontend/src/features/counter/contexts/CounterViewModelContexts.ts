import { ViewModel } from '@/lib/api/hooks/useViewModel'
import { createContext } from 'react'
import { CounterCommandEnvelope, CounterEventEnvelope, CounterState } from '../types/counterTypes'

// Counter
export const CounterStateViewModelContext = createContext<{
  state: CounterState | undefined
} | null>(null)

export const CounterViewModelContext = createContext<ViewModel<
  CounterEventEnvelope,
  CounterCommandEnvelope
> | null>(null)

// Global counter
export const GlobalCounterStateViewModelContext = createContext<{
  state: CounterState | undefined
} | null>(null)

export const GlobalCounterViewModelContext = createContext<ViewModel<
  CounterEventEnvelope,
  CounterCommandEnvelope
> | null>(null)
