export type WindowCommand = {
  command: 'messageBox'
  payload: MessageBoxCommandPayload
}

export type MessageBoxCommandPayload = {
  title?: string
  message: string
  buttons?: 'Ok' | 'OkCancel' | 'YesNo' | 'YesNoCancel'
  icon?: 'Info' | 'Warning' | 'Error' | 'Question'
}

export type WindowEvent = {
  event: 'receive:messageBox'
  payload: 'Ok' | 'Cancel' | 'Yes' | 'No'
}
