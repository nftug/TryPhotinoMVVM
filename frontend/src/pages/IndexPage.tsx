import Counter from '@/features/counter/components/Counter'
import { counterViewIdAtom } from '@/features/counter/stores/counterAtom'
import { flexCenterStyle } from '@/lib/layout/constants/styles'
import { Stack, Typography } from '@mui/material'
import { useAtomValue } from 'jotai'

const IndexPage = () => {
  const counterViewId = useAtomValue(counterViewIdAtom)

  return (
    <Stack sx={flexCenterStyle} spacing={5} height={1}>
      <Typography variant="h3">Vite + React + Photino.NET</Typography>

      <Counter />
      <Counter viewId={counterViewId} />

      <Typography variant="body1" color="textSecondary">
        The count values come from the C# side.
      </Typography>
    </Stack>
  )
}

export default IndexPage
