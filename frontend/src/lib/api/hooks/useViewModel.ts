import { Unsubscribe } from 'nanoevents'
import { useEffect, useMemo, useState } from 'react'
import { createDispatcher, createInvoker, createSubscriber, initView } from '../handlers/ipcHandler'
import type { CommandEnvelope, EventEnvelope, ViewId, ViewModelTypeName } from '../types/apiTypes'

const useViewModel = <
  TEventEnvelope extends EventEnvelope,
  TCommandEnvelope extends CommandEnvelope
>(
  viewType: ViewModelTypeName,
  viewIdShared?: ViewId
) => {
  const viewId = useMemo(() => viewIdShared ?? (crypto.randomUUID() as ViewId), [viewIdShared])

  const dispatch = useMemo(() => createDispatcher<TCommandEnvelope>(viewId), [viewId])
  const subscribe = useMemo(() => createSubscriber<TEventEnvelope>(viewId), [viewId])
  const invoke = useMemo(() => createInvoker<TEventEnvelope, TCommandEnvelope>(viewId), [viewId])
  const useViewState = useMemo(() => createViewStateHook<TEventEnvelope>(subscribe), [subscribe])

  // 初期化と破棄のコマンド発行
  useEffect(
    () => initView({ viewId, viewType, persist: !!viewIdShared }),
    [viewId, viewType, viewIdShared]
  )

  return { dispatch, subscribe, invoke, useViewState, viewId }
}

const createViewStateHook = <TEvent extends EventEnvelope>(
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
