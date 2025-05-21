import { createNanoEvents } from 'nanoevents'
import { useCallback, useEffect, useMemo, useState } from 'react'
import type { CommandPayload, EventPayload, ViewModelTypeName } from './types'
import { dispatchCommand, eventHandlerSetMap } from './viewHandler'

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
    useEffect(() => onEvent(eventType, setValue), [eventType])
    return value
  }

  useEffect(() => {
    const emitEvent = (payload: unknown) => eventEmitter.emit('event', payload as TEventPayload)

    const handlerSet = eventHandlerSetMap.get(type) ?? new Set()
    handlerSet.add(emitEvent)
    eventHandlerSetMap.set(type, handlerSet)

    dispatch({ type: 'init' } as TCommandPayload)

    return () => {
      handlerSet.delete(emitEvent)
      eventHandlerSetMap.set(type, handlerSet)
    }
  }, [dispatch, eventEmitter, type])

  return { dispatch, onEvent, useEventValue }
}

export default useViewModel
