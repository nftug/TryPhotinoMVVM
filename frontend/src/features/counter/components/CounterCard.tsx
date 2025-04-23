import { useCounterViewModel } from '@/features/counter/api/useCounterViewModel'

const CounterCard = () => {
  const { viewModel, dispatch } = useCounterViewModel()

  return (
    viewModel && (
      <div className="card">
        <div>Count is {viewModel.count}</div>
        <div style={{ marginTop: '1em' }}>
          <fieldset disabled={viewModel.isProcessing} style={{ border: 'none', padding: '0' }}>
            <button onClick={() => dispatch({ type: 'increment' })}>+</button>
            <button onClick={() => dispatch({ type: 'decrement' })} style={{ marginLeft: '0.5em' }}>
              -
            </button>
          </fieldset>
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
