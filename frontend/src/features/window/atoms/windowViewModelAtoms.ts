import useViewModel from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue, useSetAtom } from 'jotai'
import { useEffect } from 'react'
import { WindowCommandEnvelope, WindowEventEnvelope } from '../types/windowTypes'

const windowViewModelAtom = atom<WindowViewModel>()

const useWindowViewModelInternal = () =>
  useViewModel<WindowEventEnvelope, WindowCommandEnvelope>('Window')

export type WindowViewModel = ReturnType<typeof useWindowViewModelInternal>

export const useWindowViewModel = () => useAtomValue(windowViewModelAtom)

export const useProvideWindowViewModel = () => {
  const viewModel = useWindowViewModelInternal()
  const setViewModelValue = useSetAtom(windowViewModelAtom)

  useEffect(() => {
    setViewModelValue(viewModel)
  }, [viewModel, setViewModelValue])
}
