import { CommandMessage, CommandPayload, EventMessage, EventPayload, ViewId } from '../types/api'

type MessageHandler = (payload: EventPayload) => void

export const eventHandlerSetMap = new Map<ViewId, Set<MessageHandler>>()

export const initializeViewHandler = () => {
  window.external.receiveMessage((json) => {
    try {
      const { viewId, event, payload } = JSON.parse(json) as EventMessage<EventPayload>
      const eventHandlerSet = eventHandlerSetMap.get(viewId)
      eventHandlerSet?.forEach((handler) => handler({ event, payload }))
    } catch {
      console.warn('Invalid message from Photino:', json)
    }
  })
}

export const dispatchCommand = <T extends CommandPayload>(message: CommandMessage<T>) =>
  window.external.sendMessage(JSON.stringify(message))
