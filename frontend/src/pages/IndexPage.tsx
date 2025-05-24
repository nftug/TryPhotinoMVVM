import Counter from '@/features/counter/components/Counter'
import { flexCenterStyle, fullViewHeightStyle } from '@/lib/layout/constants/styles'
import { Box, Stack, Typography } from '@mui/material'

const IndexPage = () => {
  return (
    <Box sx={fullViewHeightStyle}>
      <Stack sx={flexCenterStyle} spacing={6} height={1}>
        <Typography variant="h3">Vite + React + Photino.NET</Typography>

        <Counter />

        <Typography variant="body1" color="textSecondary">
          The count values come from the C# side.
        </Typography>
      </Stack>
    </Box>
  )
}

export default IndexPage
