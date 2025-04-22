import './App.css'
import reactLogo from './assets/react.svg'
import { CounterInPayload, CounterOutPayload } from './lib/api/types'
import { usePhotinoMessage } from './lib/api/usePhotinoMessage'
import viteLogo from '/vite.svg'

function App() {
  const { payload, sendMessage } = usePhotinoMessage<CounterInPayload, CounterOutPayload>('Counter')

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
        <div>count is {payload?.count}</div>
        <div style={{ marginTop: '1em' }}>
          <button onClick={() => sendMessage({ type: 'increment' })}>+</button>
          <button
            onClick={() => sendMessage({ type: 'decrement' })}
            style={{ marginLeft: '0.5em' }}
          >
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
