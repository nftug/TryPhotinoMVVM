import { CommandId } from '@/lib/api/types/apiTypes'

export type WindowCommand = {
  command: 'messageBox'
  payload: {
    title?: string
    message: string
    buttons?: 'Ok' | 'OkCancel' | 'YesNo' | 'YesNoCancel'
    icon?: 'Info' | 'Warning' | 'Error' | 'Question'
  }
}

export type WindowEvent = {
  event: 'receive:messageBox'
  payload: 'Ok' | 'Cancel' | 'Yes' | 'No'
  commandName: 'messageBox'
  commandId: CommandId
}
