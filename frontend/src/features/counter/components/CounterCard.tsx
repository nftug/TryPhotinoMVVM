import { useCounterViewModel } from '@/features/counter/api/useCounterViewModel'
import { useEffect, useRef } from 'react'

const CounterCard = () => {
  const { viewModel, dispatch } = useCounterViewModel()
  const inputRef = useRef<HTMLInputElement>(null)

  useEffect(() => {
    if (inputRef.current && viewModel) {
      inputRef.current.value = viewModel.count.toString()
    }
  }, [viewModel])

  return (
    viewModel && (
      <div className="card">
        <div>Count is {viewModel.count}</div>

        <div style={{ marginTop: '1em' }}>
          <fieldset disabled={viewModel.isProcessing} style={{ border: 'none', padding: '0' }}>
            <button onClick={() => dispatch({ type: 'increment' })}>+</button>
            <button
              onClick={() => dispatch({ type: 'decrement' })}
              style={{ marginLeft: '0.5em' }}
              disabled={!viewModel.canDecrement}
            >
              -
            </button>
          </fieldset>
        </div>

        <div style={{ marginTop: '1em' }}>
          <input
            type="number"
            defaultValue={viewModel.count}
            ref={inputRef}
            disabled={viewModel.isProcessing}
            onBlur={() => {
              const v = inputRef.current && parseInt(inputRef.current.value)
              if (v && !isNaN(v)) dispatch({ type: 'set', payload: { value: v } })
            }}
          />
        </div>

        <p>
          {viewModel.twiceCount === null
            ? 'The result of doubling the count will be displayed here.'
            : viewModel.isProcessing
            ? 'Loading...'
            : `Doubled: ${viewModel.twiceCount}`}
        </p>
      </div>
    )
  )
}

export default CounterCard
