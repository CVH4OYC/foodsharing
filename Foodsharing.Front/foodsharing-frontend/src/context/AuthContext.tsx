import { createContext, useContext, useState, useEffect } from "react";

type AuthContextType = {
  isAuth: boolean;
  login: (token: string) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType>({
  isAuth: false,
  login: () => {},
  logout: () => {},
});

const getTokenFromCookie = (): string | null => {
  const match = document.cookie.match(/(?:^|; )token=([^;]*)/);
  return match ? decodeURIComponent(match[1]) : null;
};

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [isAuth, setIsAuth] = useState(() => !!getTokenFromCookie());

  const login = (token: string) => {
    document.cookie = `token=${encodeURIComponent(token)}; path=/; max-age=3600`; // 1 час
    setIsAuth(true);
  };

  const logout = () => {
    document.cookie = `token=; path=/; max-age=0`;
    setIsAuth(false);
  };

  return (
    <AuthContext.Provider value={{ isAuth, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
