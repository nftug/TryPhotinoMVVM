import { useCounterViewModel } from '@/features/counter/api/useCounterViewModel'
import { useEffect, useRef } from 'react'

const CounterCard = () => {
  const { state, dispatch, onEvent } = useCounterViewModel()
  const inputRef = useRef<HTMLInputElement>(null)

  useEffect(() => {
    return onEvent('fizzBuzz', (payload) => {
      alert(payload)
    })
  }, [onEvent])

  useEffect(() => {
    if (inputRef.current && state) {
      inputRef.current.value = state.count.toString()
    }
  }, [state])

  return (
    state && (
      <div className="card">
        <div>Count is {state.count}</div>

        <div style={{ marginTop: '1em' }}>
          <fieldset disabled={state.isProcessing} style={{ border: 'none', padding: '0' }}>
            <button onClick={() => dispatch({ type: 'increment' })}>+</button>
            <button
              onClick={() => dispatch({ type: 'decrement' })}
              style={{ marginLeft: '0.5em' }}
              disabled={!state.canDecrement}
            >
              -
            </button>
          </fieldset>
        </div>

        <div style={{ marginTop: '1em' }}>
          <input
            type="number"
            defaultValue={state.count}
            ref={inputRef}
            disabled={state.isProcessing}
            onBlur={() => {
              const v = inputRef.current && parseInt(inputRef.current.value)
              if (v && !isNaN(v)) dispatch({ type: 'set', payload: { value: v } })
            }}
          />
        </div>

        <p>
          {state.twiceCount === null
            ? 'The result of doubling the count will be displayed here.'
            : state.isProcessing
            ? 'Loading...'
            : `Doubled: ${state.twiceCount}`}
        </p>
      </div>
    )
  )
}

export default CounterCard
