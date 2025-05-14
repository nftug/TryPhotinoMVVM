import { createNanoEvents } from 'nanoevents'
import { useCallback, useEffect, useMemo, useState } from 'react'
import type { CommandPayload, EventPayload, ViewModelTypeName } from './types'
import { dispatchCommand, eventHandlerMap } from './viewHandler'

const useViewModel = <TCommandPayload extends CommandPayload, TEventPayload extends EventPayload>(
  type: ViewModelTypeName
) => {
  const eventEmitter = useMemo(
    () => createNanoEvents<{ event: (payload: TEventPayload) => void }>(),
    []
  )

  const dispatch = useCallback(
    (payload: TCommandPayload) => dispatchCommand({ type, payload }),
    [type]
  )

  const onEvent = <T extends TEventPayload['type']>(
    type: T,
    cb: (payload: Extract<TEventPayload, { type: T }>['payload']) => void
  ) =>
    eventEmitter.on('event', ({ type: eventType, payload: eventPayload }) => {
      if (eventType !== type) return
      cb(eventPayload as Extract<TEventPayload, { type: T }>)
    })

  const useEventValue = <T extends TEventPayload['type']>(
    eventType: T
  ): Extract<TEventPayload, { type: T }>['payload'] | undefined => {
    const [value, setValue] = useState<Extract<TEventPayload, { type: T }>['payload']>()
    useEffect(() => {
      const unsubscribe = onEvent(eventType, setValue)
      return () => unsubscribe()
    }, [eventType])
    return value
  }

  useEffect(() => {
    if (!eventHandlerMap.has(type)) {
      eventHandlerMap.set(type, (payload) => eventEmitter.emit('event', payload as TEventPayload))
      dispatch({ type: 'init' } as TCommandPayload)
    }
  }, [dispatch, eventEmitter, type])

  return { dispatch, onEvent, useEventValue }
}

export default useViewModel
