// Default events and commands
export type AppEventEnvelope = {
  event: 'error'
  payload: ViewModelErrorEventResult
}

export type AppCommandEnvelope =
  | { command: 'init'; payload: { type: string } }
  | { command: 'leave' }

// DTO
export type ViewModelErrorEventResult = {
  message: string
  details?: string
}
