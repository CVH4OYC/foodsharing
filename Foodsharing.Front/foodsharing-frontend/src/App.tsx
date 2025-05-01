// App.tsx
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./components/Layout";
import Login from "./pages/Login";
import Secure from "./pages/Secure";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/login" element={<Login />} />
          <Route path="/secure" element={<Secure />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;