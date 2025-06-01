import { ViewId } from '@/lib/api/types/apiTypes'
import { flexCenterStyle } from '@/lib/layout/constants/styles'
import { Box, Button, Stack, TextField, Typography } from '@mui/material'
import { useSnackbar } from 'notistack'
import { useEffect, useRef } from 'react'
import { match, P } from 'ts-pattern'
import { useCounterViewModel } from '../hooks/useCounterViewModel'

interface Props {
  viewId?: ViewId
}

const Counter = ({ viewId }: Props) => {
  const { dispatch, subscribe, state } = useCounterViewModel(viewId)

  const { enqueueSnackbar } = useSnackbar()
  const inputRef = useRef<HTMLInputElement>(null)

  useEffect(() => {
    if (!inputRef.current || !state) return
    inputRef.current.value = state.count.toString()
  }, [state])

  useEffect(() => {
    return subscribe('fizzBuzz', ({ result }) => {
      enqueueSnackbar(result, { autoHideDuration: 1500 })
    })
  }, [enqueueSnackbar, subscribe])

  if (!state) return null

  return (
    <Stack sx={flexCenterStyle} gap={2}>
      <Typography variant="body1">Count is {state.count}</Typography>

      <Stack direction="row" gap={2}>
        <Button
          variant="contained"
          onClick={() => dispatch('increment')}
          disabled={state.isProcessing}
        >
          +
        </Button>
        <Button
          variant="contained"
          onClick={() => dispatch('decrement')}
          disabled={!state.canDecrement || state.isProcessing}
        >
          -
        </Button>
      </Stack>

      <Box sx={{ mt: 2 }}>
        <TextField
          inputRef={inputRef}
          type="number"
          label="Set Count"
          disabled={state.isProcessing}
          defaultValue={state.count}
          onBlur={(e) => {
            const value = parseInt(e.target.value)
            if (!isNaN(value)) dispatch('set', { value })
          }}
        />
      </Box>

      <Typography variant="body1" sx={{ mt: 2 }}>
        {match(state)
          .with({ isProcessing: true }, () => 'Loading...')
          .with({ twiceCount: P.nonNullable.select() }, (doubled) => `Doubled: ${doubled}`)
          .otherwise(() => 'The result of doubling the count will be displayed here.')}
      </Typography>
    </Stack>
  )
}

export default Counter
