import { CommandMessage, CommandPayload, EventMessage, ViewModelTypeName } from './types'

type MessageHandler = (payload: unknown) => void

export const eventHandlerSetMap = new Map<ViewModelTypeName, Set<MessageHandler>>()

export const initializeViewHandler = () => {
  window.external.receiveMessage((json) => {
    try {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const msg: EventMessage<any> = JSON.parse(json)
      const { type, payload } = msg
      const eventHandlerSet = eventHandlerSetMap.get(type)
      eventHandlerSet?.forEach((handler) => handler(payload))
    } catch {
      console.warn('Invalid message from Photino:', json)
    }
  })
}

export const dispatchCommand = <T extends CommandPayload>(message: CommandMessage<T>) =>
  window.external.sendMessage(JSON.stringify(message))
