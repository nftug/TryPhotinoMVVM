import { fullViewHeightStyle } from '@/lib/layout/constants/styles'
import { Box, Typography } from '@mui/material'

const AboutPage = () => {
  return (
    <Box sx={fullViewHeightStyle}>
      <Box display="flex" justifyContent="center" alignItems="center" height={1}>
        <Typography variant="h2" color="textSecondary">
          {import.meta.env.VITE_APP_NAME}
        </Typography>
      </Box>
    </Box>
  )
}

export default AboutPage
