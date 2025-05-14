import { useEffect } from 'react'
import { ViewModelTypeName } from './types'
import useViewModel from './useViewModel'

export const useErrorHandler = () => {
  const { onEvent } = useViewModel<never, ErrorEventPayload>('Error' as ViewModelTypeName)

  useEffect(() => {
    const unsubscribe = onEvent('error', (payload) => {
      console.error(payload.message)
    })
    return () => unsubscribe()
  }, [onEvent])
}

export type ErrorEventPayload = {
  type: 'error'
  payload: { type: ViewModelTypeName; message: string }
}
