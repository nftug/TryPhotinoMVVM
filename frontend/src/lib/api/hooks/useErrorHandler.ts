import { useEffect } from 'react'
import { ViewModelTypeName } from '../types/api'
import useViewModel from './useViewModel'

export const useErrorHandler = () => {
  const { onEvent } = useViewModel<ErrorEventPayload, never>('Error' as ViewModelTypeName)

  useEffect(() => {
    return onEvent('error', (payload) => {
      console.error(payload.message)
    })
  }, [onEvent])
}

export type ErrorEventPayload = {
  event: 'error'
  payload: { type: ViewModelTypeName; message: string }
}
