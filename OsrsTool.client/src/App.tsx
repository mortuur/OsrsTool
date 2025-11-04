import { Routes, Route, Link } from 'react-router-dom';
import Home from './pages/Home';
import About from './pages/About';

function App() {
  return (
    <div className="min-h-screen bg-gray-900 text-gray-100">
      <nav className="p-4 flex gap-4">
        <Link className="text-green-300" to="/">Home</Link>
        <Link className="text-green-300" to="/about">About</Link>
      </nav>

      <main className="flex items-center justify-center p-8">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;
