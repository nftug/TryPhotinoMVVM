import useViewModel from '@/lib/api/hooks/useViewModel'
import { WindowCommandEnvelope, WindowEventEnvelope } from '../types/windowTypes'

const useWindowViewModel = () => useViewModel<WindowEventEnvelope, WindowCommandEnvelope>('Window')

export default useWindowViewModel
