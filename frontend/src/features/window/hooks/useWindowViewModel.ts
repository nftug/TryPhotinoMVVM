import useViewModel from '@/lib/api/hooks/useViewModel'
import { WindowCommand, WindowEvent } from '../types/windowTypes'

const useWindowViewModel = () => useViewModel<WindowEvent, WindowCommand>('Window')

export default useWindowViewModel
