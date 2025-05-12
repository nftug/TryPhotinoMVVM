import { useCallback, useEffect, useState } from 'react'
import type { CommandPayload, ViewModelTypeName } from './types'
import { dispatchCommand, registerViewHandler, unregisterViewHandler } from './viewHandler'

const useViewModel = <TViewModel, TCommandPayload extends CommandPayload>(
  type: ViewModelTypeName
) => {
  const [viewModel, setViewModel] = useState<TViewModel>()

  const dispatch = useCallback(
    (payload: TCommandPayload) => dispatchCommand({ type, payload }),
    [type]
  )

  useEffect(() => {
    const handleReceiveViewModel = (payload: unknown) => {
      setViewModel(payload as TViewModel)
    }
    registerViewHandler(type, handleReceiveViewModel)

    dispatch({ type: 'init' } as TCommandPayload)

    return () => {
      unregisterViewHandler(type, handleReceiveViewModel)
    }
  }, [type, dispatch])

  return { viewModel, dispatch }
}

export default useViewModel
