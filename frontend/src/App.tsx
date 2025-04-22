import './App.css'
import reactLogo from './assets/react.svg'
import { CounterCommand, CounterViewModel } from './lib/api/types'
import useViewModel from './lib/api/useViewModel'
import viteLogo from '/vite.svg'

function App() {
  const { viewModel, dispatch } = useViewModel<CounterViewModel, CounterCommand>('Counter')

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <div>count is {viewModel?.count}</div>
        <div style={{ marginTop: '1em' }}>
          <button onClick={() => dispatch({ type: 'increment' })}>+</button>
          <button onClick={() => dispatch({ type: 'decrement' })} style={{ marginLeft: '0.5em' }}>
            -
          </button>
        </div>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">Click on the Vite and React logos to learn more</p>
    </>
  )
}

export default App
