import reactLogo from '@/assets/react.svg'
import CounterCard from '@/features/counter/components/CounterCard'
import './App.css'
import { useErrorHandler } from './lib/api/useErrorHandler'
import viteLogo from '/vite.svg'

function App() {
  useErrorHandler()

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
      <h1>Vite + React + Photino.NET</h1>

      <CounterCard />

      <p className="read-the-docs">The count values come from the C# side.</p>
    </>
  )
}

export default App
