import { createNanoEvents } from 'nanoevents'
import { CommandMessage, CommandPayload, EventMessage } from '../types/api'
import { EventPayload, ViewId } from './../types/api.d'

export const dispatchCommand = <T extends CommandPayload>(message: CommandMessage<T>) =>
  window.external.sendMessage(JSON.stringify(message))

const eventEmitter = createNanoEvents<Record<string, (payload: unknown) => void>>()

export const initializeEventHandler = () => {
  window.external.receiveMessage((json) => {
    try {
      const { viewId, event, payload } = JSON.parse(json) as EventMessage<EventPayload>
      eventEmitter.emit(`${viewId}:${event}`, payload)
    } catch {
      console.warn('Invalid message from Photino:', json)
    }
  })
}

export const addEventHandler = <TEvent extends EventPayload, TName extends TEvent['event']>(
  viewId: ViewId,
  eventName: TName,
  callback: (payload: Extract<TEvent, { event: TName }>['payload']) => void
) => {
  const key = `${viewId}:${eventName}`
  const handler = (payload: EventPayload['payload']) => {
    callback(payload as Extract<TEvent, { event: TName }>['payload'])
  }
  return eventEmitter.on(key, handler)
}
