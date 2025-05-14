import { CommandMessage, CommandPayload, EventMessage, ViewModelTypeName } from './types'

type MessageHandler = (payload: unknown) => void

export const eventHandlerMap = new Map<ViewModelTypeName, MessageHandler>()

export const initializeViewHandler = () => {
  window.external.receiveMessage((json) => {
    try {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const msg: EventMessage<any> = JSON.parse(json)
      const { type, payload } = msg
      const eventHandler = eventHandlerMap.get(type)
      if (eventHandler) eventHandler(payload)
    } catch {
      console.warn('Invalid message from Photino:', json)
    }
  })
}

export const dispatchCommand = <T extends CommandPayload>(message: CommandMessage<T>) =>
  window.external.sendMessage(JSON.stringify(message))
