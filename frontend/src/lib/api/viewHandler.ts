import { CommandMessage, CommandPayload, ViewModelMessage, ViewModelTypeName } from './types'

type MessageHandler = (payload: unknown) => void

const handlerMap = new Map<ViewModelTypeName, Set<MessageHandler>>()

export const initializeViewHandler = () => {
  window.external.receiveMessage((json) => {
    try {
      const msg: ViewModelMessage<unknown> = JSON.parse(json)
      const handlers = handlerMap.get(msg.type)
      if (handlers) {
        handlers.forEach((handler) => handler(msg.payload))
      }
    } catch {
      console.warn('Invalid message from Photino:', json)
    }
  })
}

export const registerViewHandler = (type: ViewModelTypeName, handler: MessageHandler) => {
  if (!handlerMap.has(type)) {
    handlerMap.set(type, new Set())
  }
  handlerMap.get(type)!.add(handler)
}

export const unregisterViewHandler = (type: ViewModelTypeName, handler: MessageHandler) => {
  handlerMap.get(type)?.delete(handler)
}

export const dispatchCommand = <T extends CommandPayload<T>>(message: CommandMessage<T>) => {
  window.external.sendMessage(JSON.stringify(message))
}
