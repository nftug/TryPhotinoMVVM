import { useCallback, useEffect, useMemo } from 'react'
import { addEventHandler, dispatchCommand } from '../stores/viewHandler'
import type {
  CommandPayload,
  DefaultCommand,
  DefaultEvent,
  EventPayload,
  ViewId,
  ViewModelTypeName
} from '../types/api'

const useViewModel = <TEvent extends EventPayload, TCommand extends CommandPayload>(
  viewType: ViewModelTypeName,
  viewIdShared?: ViewId
) => {
  const viewId = useMemo<ViewId>(
    () => viewIdShared ?? (crypto.randomUUID() as ViewId),
    [viewIdShared]
  )

  const dispatch = useCallback(
    (command: TCommand) => dispatchCommand({ viewId, ...command }),
    [viewId]
  )

  const onEvent = useCallback(
    <TName extends TEvent['event']>(
      eventName: TName,
      callback: (payload: Extract<TEvent, { event: TName }>['payload']) => void
    ) => addEventHandler(viewId, eventName, callback),
    [viewId]
  )

  // 初期化と破棄のコマンド発行
  useEffect(() => {
    dispatchCommand<DefaultCommand>({ viewId, command: 'init', payload: { type: viewType } })

    return () => {
      if (!viewIdShared) {
        dispatchCommand<DefaultCommand>({ viewId, command: 'leave' })
      }
    }
  }, [viewId, viewType, viewIdShared])

  // エラーハンドリング
  useEffect(() => {
    return addEventHandler<DefaultEvent, 'error'>(viewId, 'error', (payload) =>
      console.error(payload.message)
    )
  }, [viewId])

  return { dispatch, onEvent, viewId }
}

export default useViewModel
