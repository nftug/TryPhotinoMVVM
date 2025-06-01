import Counter from '@/features/counter/components/Counter'
import { counterViewIdAtom } from '@/features/counter/stores/counterAtom'
import useWindowViewModel from '@/features/window/hooks/useWindowViewModel'
import { flexCenterStyle } from '@/lib/layout/constants/styles'
import { Button, Stack, Typography } from '@mui/material'
import { useAtomValue } from 'jotai'

const IndexPage = () => {
  const counterViewId = useAtomValue(counterViewIdAtom)
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

      <Counter />
      <Counter viewId={counterViewId} />

      <Typography variant="body1" color="textSecondary">
        The count values come from the C# side.
      </Typography>
    </Stack>
  )
}

export default IndexPage
