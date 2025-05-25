import Counter from '@/features/counter/components/Counter'
import { flexCenterStyle } from '@/lib/layout/constants/styles'
import { Stack, Typography } from '@mui/material'

const IndexPage = () => {
  return (
    <Stack sx={flexCenterStyle} spacing={5} height={1}>
      <Typography variant="h3">Vite + React + Photino.NET</Typography>

      <Counter />

      <Typography variant="body1" color="textSecondary">
        The count values come from the C# side.
      </Typography>
    </Stack>
  )
}

export default IndexPage
