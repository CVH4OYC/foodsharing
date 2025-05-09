import { createContext, useContext, useState, useEffect, useCallback } from "react";
import { API } from "../services/api";

type AuthContextType = {
  isAuth: boolean;
  isLoading: boolean;
  checkAuth: () => Promise<void>;
  logout: () => Promise<void>;
};

const AuthContext = createContext<AuthContextType>({
  isAuth: false,
  isLoading: true,
  checkAuth: async () => {},
  logout: async () => {},
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [isAuth, setIsAuth] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  const checkAuth = useCallback(async () => {
    setIsLoading(true);
    try {
      const res = await API.get("/user/me", { withCredentials: true });
      setIsAuth(res.status === 200);
    } catch {
      setIsAuth(false);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const logout = async () => {
    try {
      await API.post("/user/logout", {}, { withCredentials: true });
    } catch {}
    setIsAuth(false);
  };

  useEffect(() => {
    checkAuth();
  }, [checkAuth]);

  if (isLoading) return null; // Ждём проверку авторизации перед рендером

  return (
    <AuthContext.Provider value={{ isAuth, isLoading, checkAuth, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
