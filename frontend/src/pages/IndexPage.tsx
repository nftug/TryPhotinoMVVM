import Counter from '@/features/counter/components/Counter'
import { CounterViewModelProvider } from '@/features/counter/providers/CounterViewModelProvider'
import {
  useCounterViewModelContext,
  useGlobalCounterViewModelContext
} from '@/features/counter/providers/useCounterViewModelContext'
import useWindowViewModel from '@/features/window/hooks/useWindowViewModel'
import { flexCenterStyle } from '@/lib/layout/constants/styles'
import { Button, Stack, Typography } from '@mui/material'

const IndexPage = () => {
  const { invoke, dispatch } = useWindowViewModel()

  const handleClickButton = async () => {
    const dialogResult = await invoke('messageBox', {
      message: 'Press OK or Cancel',
      title: 'Dialog test',
      buttons: 'OkCancel',
      icon: 'Warning'
    })
    dispatch('messageBox', { message: `You pressed ${dialogResult}` })
  }

  return (
    <Stack sx={flexCenterStyle} spacing={5}>
      <Typography variant="h3">Vite + React + Photino.NET</Typography>

      <Button variant="contained" onClick={handleClickButton}>
        Click me!
      </Button>

      <CounterViewModelProvider>
        <Counter useCounterViewModel={useCounterViewModelContext} />
      </CounterViewModelProvider>

      <Counter useCounterViewModel={useGlobalCounterViewModelContext} />

      <Typography variant="body1" color="textSecondary">
        The count values come from the C# side.
      </Typography>
    </Stack>
  )
}

export default IndexPage
