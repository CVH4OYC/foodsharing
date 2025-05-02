// src/context/AuthContext.tsx
import { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext({
  isAuth: false,
  updateAuth: () => {}
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [isAuth, setIsAuth] = useState(false);

  const updateAuth = () => {
    setIsAuth(!!localStorage.getItem("token"));
  };

  useEffect(() => {
    updateAuth();
  }, []);

  return (
    <AuthContext.Provider value={{ isAuth, updateAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);