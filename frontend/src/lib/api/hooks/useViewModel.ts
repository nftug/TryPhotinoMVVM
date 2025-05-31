import { Unsubscribe } from 'nanoevents'
import { useEffect, useMemo, useState } from 'react'
import {
  createCommandDispatcher,
  createCommandInvoker,
  createEventSubscriber,
  initView
} from '../stores/viewHandler'
import type { CommandPayload, EventPayload, ViewId, ViewModelTypeName } from '../types/apiTypes'

const useViewModel = <TEvent extends EventPayload, TCommand extends CommandPayload>(
  viewType: ViewModelTypeName,
  viewIdShared?: ViewId
) => {
  const viewId = useMemo(() => viewIdShared ?? (crypto.randomUUID() as ViewId), [viewIdShared])

  const dispatchCommand = useMemo(() => createCommandDispatcher<TCommand>(viewId), [viewId])
  const subscribeEvent = useMemo(() => createEventSubscriber<TEvent>(viewId), [viewId])
  const invokeCommand = useMemo(() => createCommandInvoker<TEvent, TCommand>(viewId), [viewId])
  const useViewState = useMemo(() => createViewStateHook<TEvent>(subscribeEvent), [subscribeEvent])

  // 初期化と破棄のコマンド発行
  useEffect(
    () => initView({ viewId, viewType, persist: !!viewIdShared }),
    [viewId, viewType, viewIdShared]
  )

  return { dispatchCommand, subscribeEvent, invokeCommand, useViewState, viewId }
}

const createViewStateHook = <TEvent extends EventPayload>(
  subscribeEvent: <TName extends TEvent['event']>(
    eventName: TName,
    callback: (payload: Extract<TEvent, { event: TName }>['payload']) => void
  ) => Unsubscribe
) => {
  return function useViewState<TName extends TEvent['event']>(eventName: TName) {
    const [state, setState] = useState<Extract<TEvent, { event: TName }>['payload']>()
    useEffect(() => subscribeEvent(eventName, setState), [eventName])
    return state
  }
}

export default useViewModel
