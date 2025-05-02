// src/App.tsx
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./components/Layout";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Secure from "./pages/Secure";
import Profile from "./pages/Profile";
import AdsPage from "./pages/AdsPage";
import { AuthProvider } from "./context/AuthContext";

function App() {
  return (
    <AuthProvider>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
        <Route path="/secure" element={<Secure />} />
        <Route path="login" element={<Login />} />
        <Route path="register" element={<Register />} />
        <Route path="profile" element={<Profile />} />
        <Route path="/ads" element={<AdsPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
    </AuthProvider>
  );
}

export default App;