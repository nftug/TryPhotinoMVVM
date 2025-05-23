import TheDrawer from '@/lib/layout/components/TheDrawer'
import TheHeader from '@/lib/layout/components/TheHeader'
import AboutPage from '@/pages/AboutPage'
import IndexPage from '@/pages/IndexPage'
import SettingsPage from '@/pages/SettingsPage'
import { Box, createTheme, CssBaseline, ThemeProvider, Toolbar } from '@mui/material'
import { SnackbarProvider } from 'notistack'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { HeaderProvider } from './lib/layout/components/HeaderContext'

const theme = createTheme({ colorSchemes: { dark: true } })

const App = () => {
  return (
    <BrowserRouter>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <SnackbarProvider
          anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
          preventDuplicate
        >
          <HeaderProvider>
            <TheHeader />
            <TheDrawer />
          </HeaderProvider>

          <Box component="main">
            <Toolbar />
            <Routes>
              <Route index element={<IndexPage />} />
              <Route path="/about" element={<AboutPage />} />
              <Route path="/settings" element={<SettingsPage />} />
            </Routes>
          </Box>
        </SnackbarProvider>
      </ThemeProvider>
    </BrowserRouter>
  )
}

export default App
