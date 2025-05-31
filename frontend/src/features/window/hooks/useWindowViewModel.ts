import useViewModel from '@/lib/api/hooks/useViewModel'
import { useCallback } from 'react'
import { MessageBoxCommandPayload, WindowCommand, WindowEvent } from '../types/windowTypes'

const useWindowViewModel = () => {
  const { invokeCommand } = useViewModel<WindowEvent, WindowCommand>('Window')

  const showMessageBox = useCallback(
    (payload: MessageBoxCommandPayload) => invokeCommand('messageBox', payload),
    [invokeCommand]
  )

  return { showMessageBox }
}

export default useWindowViewModel
