import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { Home } from "./pages/Home";
import { SecurePage } from "./pages/SecurePage";
import { Login } from "./components/Login";

function App() {
  return (
    <BrowserRouter>
      <nav>
        <Link to="/">Главная</Link> | <Link to="/login">Вход</Link> | <Link to="/secure">Секретка</Link>
      </nav>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/secure" element={<SecurePage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
