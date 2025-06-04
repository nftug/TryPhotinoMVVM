import { ScopeProvider } from 'jotai-scope'
import { ReactNode } from 'react'
import { counterViewModelAtom, useProvideCounterViewModel } from '../atoms/counterViewModelAtom'

const CounterViewModelScopedProvider = ({ children }: { children?: ReactNode }) => {
  return (
    <ScopeProvider atoms={[counterViewModelAtom]}>
      <ProviderInternal>{children}</ProviderInternal>
    </ScopeProvider>
  )
}

const ProviderInternal = ({ children }: { children?: ReactNode }) => {
  useProvideCounterViewModel()
  return children
}

export default CounterViewModelScopedProvider
