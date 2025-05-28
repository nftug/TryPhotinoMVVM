import { createNanoEvents } from 'nanoevents'
import { useCallback, useEffect, useMemo } from 'react'
import { dispatchCommand, eventHandlerSetMap } from '../stores/viewHandler'
import type { CommandPayload, EventPayload, ViewModelTypeName } from '../types/api'

const useViewModel = <TEventPayload extends EventPayload, TCommandPayload extends CommandPayload>(
  type: ViewModelTypeName
) => {
  const eventEmitter = useMemo(
    () => createNanoEvents<{ event: (payload: TEventPayload) => void }>(),
    []
  )

  const dispatch = useCallback(
    (command: CommandPayload) => dispatchCommand({ type, ...command }),
    [type]
  )

  const onEvent = <T extends TEventPayload['event']>(
    event: T,
    cb: (payload: Extract<TEventPayload, { event: T }>['payload']) => void
  ) =>
    eventEmitter.on('event', ({ event: eventName, payload: eventPayload }) => {
      if (eventName !== event) return
      cb(eventPayload as Extract<TEventPayload, { event: T }>['payload'])
    })

  useEffect(() => {
    const emitEvent = (payload: EventPayload) =>
      eventEmitter.emit('event', payload as TEventPayload)

    const handlerSet = eventHandlerSetMap.get(type) ?? new Set()
    handlerSet.add(emitEvent)
    eventHandlerSetMap.set(type, handlerSet)

    dispatch({ command: 'init' } as TCommandPayload)

    return () => {
      handlerSet.delete(emitEvent)
      eventHandlerSetMap.set(type, handlerSet)
    }
  }, [dispatch, eventEmitter, type])

  return { dispatch, onEvent }
}

export default useViewModel
