import Counter from '@/features/counter/components/Counter'
import CounterViewModelScopedProvider from '@/features/counter/providers/CounterViewModelScopedProvider'
import { useWindowViewModel } from '@/features/window/atoms/windowViewModelAtoms'
import { flexCenterStyle } from '@/lib/layout/constants/styles'
import { Button, Stack, Typography } from '@mui/material'

const IndexPage = () => {
  const window = useWindowViewModel()

  const handleClickButton = async () => {
    const dialogResult = await window?.invoke('messageBox', {
      message: 'Press OK or Cancel',
      title: 'Dialog test',
      buttons: 'OkCancel',
      icon: 'Warning'
    })
    window?.dispatch('messageBox', { message: `You pressed ${dialogResult}` })
  }

  return (
    <Stack sx={flexCenterStyle} spacing={5}>
      <Typography variant="h3">Vite + React + Photino.NET</Typography>

      <Button variant="contained" onClick={handleClickButton}>
        Click me!
      </Button>

      <CounterViewModelScopedProvider>
        <Counter />
      </CounterViewModelScopedProvider>

      <Counter />

      <Typography variant="body1" color="textSecondary">
        The count values come from the C# side.
      </Typography>
    </Stack>
  )
}

export default IndexPage
