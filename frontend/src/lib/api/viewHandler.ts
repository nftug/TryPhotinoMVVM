import { atom } from 'jotai'
import { atomFamily } from 'jotai/utils'
import { CommandMessage, CommandPayload, ViewModelMessage, ViewModelTypeName } from './types'

type MessageHandler = (payload: unknown) => void

export const stateHandlerMap = new Map<ViewModelTypeName, MessageHandler>()

export const eventHandlerMap = new Map<ViewModelTypeName, MessageHandler>()

export const viewModelsFamily = atomFamily(() => atom<unknown>())

export const initializeViewHandler = () => {
  window.external.receiveMessage((json) => {
    try {
      const msg: ViewModelMessage<unknown> = JSON.parse(json)
      const { type, payload } = msg

      if (type.endsWith('_Event')) {
        const key = type.replace(/_Event$/, '') as ViewModelTypeName
        const eventHandler = eventHandlerMap.get(key)
        if (eventHandler) eventHandler(payload)
      } else {
        const stateHandler = stateHandlerMap.get(type as ViewModelTypeName)
        if (stateHandler) stateHandler(payload)
      }
    } catch {
      console.warn('Invalid message from Photino:', json)
    }
  })
}

export const dispatchCommand = <T extends CommandPayload>(message: CommandMessage<T>) =>
  window.external.sendMessage(JSON.stringify(message))
