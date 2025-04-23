import { useCounterViewModel } from '@/features/counter/api/useCounterViewModel'

const CounterCard = () => {
  const { viewModel, dispatch } = useCounterViewModel()

  return (
    <div className="card">
      <div>count is {viewModel?.count}</div>
      <div style={{ marginTop: '1em' }}>
        <button onClick={() => dispatch({ type: 'increment' })}>+</button>
        <button onClick={() => dispatch({ type: 'decrement' })} style={{ marginLeft: '0.5em' }}>
          -
        </button>
      </div>
      <p>The count values come from the C# side.</p>
    </div>
  )
}

export default CounterCard
