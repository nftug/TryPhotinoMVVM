import { PrimitiveAtom, useAtom } from 'jotai'
import { createNanoEvents } from 'nanoevents'
import { useCallback, useEffect, useMemo } from 'react'
import type { CommandPayload, EventPayload, ViewModelTypeName } from './types'
import { dispatchCommand, eventHandlerMap, stateHandlerMap, viewModelsFamily } from './viewHandler'

const useViewModel = <
  TState,
  TCommandPayload extends CommandPayload,
  TEventPayload extends EventPayload
>(
  type: ViewModelTypeName
) => {
  const [state, setState] = useAtom(viewModelsFamily(type) as PrimitiveAtom<TState | undefined>)

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

  useEffect(() => {
    if (!stateHandlerMap.has(type)) {
      stateHandlerMap.set(type, (x) => setState(x as TState))
      dispatch({ type: 'init' } as TCommandPayload)
    }

    if (!eventHandlerMap.has(type)) {
      eventHandlerMap.set(type, (payload) => eventEmitter.emit('event', payload as TEventPayload))
    }
  }, [dispatch, eventEmitter, setState, type])

  return { state, dispatch, onEvent }
}

export default useViewModel
