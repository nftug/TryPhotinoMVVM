import { useEffect } from 'react'
import { CommandPayload, ViewModelTypeName } from './types'
import useViewModel from './useViewModel'

export const useErrorHandler = () => {
  const { viewModel } = useViewModel<ErrorViewModel, CommandPayload>('Error' as ViewModelTypeName)

  useEffect(() => {
    if (!viewModel) return
    alert(viewModel.message)
  }, [viewModel])
}

export type ErrorViewModel = { message: string }
