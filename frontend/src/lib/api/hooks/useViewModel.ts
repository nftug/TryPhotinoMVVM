import { createNanoEvents } from 'nanoevents'
import { useCallback, useEffect, useMemo } from 'react'
import { dispatchCommand, eventHandlerSetMap } from '../stores/viewHandler'
import type { CommandPayload, EventPayload, ViewId, ViewModelTypeName } from '../types/api'

const useViewModel = <TEventPayload extends EventPayload, TCommandPayload extends CommandPayload>(
  viewType: ViewModelTypeName,
  viewIdShared?: ViewId
) => {
  const eventEmitter = useMemo(
    () => createNanoEvents<{ event: (payload: TEventPayload) => void }>(),
    []
  )
  const viewId = useMemo<ViewId>(
    () => viewIdShared ?? (crypto.randomUUID() as ViewId),
    [viewIdShared]
  )

  const dispatch = useCallback(
    (command: TCommandPayload) => dispatchCommand({ viewId, ...command }),
    [viewId]
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

    const handlerSet = eventHandlerSetMap.get(viewId) ?? new Set()
    handlerSet.add(emitEvent)
    eventHandlerSetMap.set(viewId, handlerSet)

    dispatch({ command: 'init', payload: { type: viewType } } as TCommandPayload)

    return () => {
      handlerSet.delete(emitEvent)
      eventHandlerSetMap.set(viewId, handlerSet)

      // viewIdを共有中の場合は自動で破棄しない
      if (!viewIdShared) {
        dispatch({ command: 'leave' } as TCommandPayload)
      }
    }
  }, [dispatch, eventEmitter, viewId, viewType, viewIdShared])

  return { dispatch, onEvent, viewId }
}

export default useViewModel
