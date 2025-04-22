import { IncomingMessage, ViewModelType } from './types'

type MessageHandler<T = unknown> = (payload: T) => void

let photinoReceiverRegistered = false

const handlerMap = new Map<ViewModelType, MessageHandler>()

export const registerPhotinoHandler = (type: ViewModelType, handler: MessageHandler) => {
  handlerMap.set(type, handler)
}

export const unregisterPhotinoHandler = (type: ViewModelType) => {
  handlerMap.delete(type)
}

export const initializePhotinoReceiver = () => {
  if (photinoReceiverRegistered) return
  photinoReceiverRegistered = true

  window.external.receiveMessage((json) => {
    try {
      const msg: IncomingMessage<unknown> = JSON.parse(json)
      const handler = handlerMap.get(msg.type)
      if (handler) {
        handler(msg.payload)
      }
    } catch {
      console.warn('Invalid message from Photino:', json)
    }
  })
}
