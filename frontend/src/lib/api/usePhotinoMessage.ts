import { useCallback, useEffect, useState } from 'react'
import { initializePhotinoReceiver, registerPhotinoHandler } from './photinoReceiver'
import type { OutgoingMessage, OutgoingSubMessage, ViewModelType } from './types'

export const usePhotinoMessage = <TIn, TOut extends OutgoingSubMessage<TOut>>(
  viewModelType: ViewModelType
) => {
  const [payload, setPayload] = useState<TIn>()

  const sendMessage = useCallback(
    (message: TOut) => {
      const payload: OutgoingMessage<TOut> = {
        type: viewModelType,
        payload: message,
      }
      window.external.sendMessage(JSON.stringify(payload))
    },
    [viewModelType]
  )

  useEffect(() => {
    initializePhotinoReceiver()
    registerPhotinoHandler(viewModelType, (payload) => setPayload(payload as TIn))

    sendMessage({ type: 'init' } as TOut)
  }, [viewModelType, sendMessage])

  return { payload, sendMessage }
}
