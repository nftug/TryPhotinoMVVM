import { useAtom } from 'jotai'
import { createNanoEvents } from 'nanoevents'
import { useCallback, useEffect, useMemo } from 'react'
import type { CommandPayload, EventPayload, ViewModelTypeName } from './types'
import { dispatchCommand, eventHandlerMap, stateHandlerMap, viewModelsFamily } from './viewHandler'

const useViewModel = <
  TViewModel,
  TCommandPayload extends CommandPayload,
  TEventPayload extends EventPayload
>(
  type: ViewModelTypeName
) => {
  const [viewModel, setViewModel] = useAtom(viewModelsFamily(type))
  const notificationEvent = useMemo(
    () => createNanoEvents<{ notification: (payload: TEventPayload) => void }>(),
    []
  )

  const dispatch = useCallback(
    (payload: TCommandPayload) => dispatchCommand({ type, payload }),
    [type]
  )

  const onEvent = useCallback(
    <T extends TEventPayload['type']>(
      type: T,
      cb: (payload: Extract<TEventPayload, { type: T }>['payload']) => void
    ) =>
      notificationEvent.on('notification', ({ type: eventType, payload: eventPayload }) => {
        if (eventType !== type) return
        cb(eventPayload as Extract<TEventPayload, { type: T }>)
      }),
    [notificationEvent]
  )

  useEffect(() => {
    if (!stateHandlerMap.has(type)) {
      console.log(`state set: ${type}`)
      stateHandlerMap.set(type, setViewModel)
      dispatch({ type: 'init' } as TCommandPayload)
    }

    if (!eventHandlerMap.has(type)) {
      console.log(`event set: ${type}`)
      eventHandlerMap.set(type, (payload) =>
        notificationEvent.emit('notification', payload as TEventPayload)
      )
    }
  }, [dispatch, notificationEvent, setViewModel, type])

  return { viewModel: viewModel as TViewModel, dispatch, onEvent }
}

export default useViewModel
