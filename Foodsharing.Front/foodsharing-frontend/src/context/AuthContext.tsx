import {
  createContext,
  useContext,
  useState,
  useEffect,
  useCallback,
} from "react";
import { API } from "../services/api";

type AuthContextType = {
  isAuth: boolean;
  isLoading: boolean;
  userId: string | null;
  roles: string[];
  checkAuth: () => Promise<void>;
  logout: () => Promise<void>;
  hasRole: (role: string) => boolean;
};

const AuthContext = createContext<AuthContextType>({
  isAuth: false,
  isLoading: true,
  userId: null,
  roles: [],
  checkAuth: async () => {},
  logout: async () => {},
  hasRole: () => false,
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [isAuth, setIsAuth] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [userId, setUserId] = useState<string | null>(null);
  const [roles, setRoles] = useState<string[]>([]);

  const checkAuth = useCallback(async () => {
    setIsLoading(true);
    try {
      const res = await API.get("/user/me", { withCredentials: true });
      if (res.status === 200) {
        setIsAuth(true);
        setUserId(res.data.userId || null);
        setRoles(res.data.roles || []);
      } else {
        setIsAuth(false);
        setUserId(null);
        setRoles([]);
      }
    } catch {
      setIsAuth(false);
      setUserId(null);
      setRoles([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const logout = async () => {
    try {
      await API.post("/user/logout", {}, { withCredentials: true });
    } catch {}
    setIsAuth(false);
    setUserId(null);
    setRoles([]);
  };

  const hasRole = (role: string) => roles.includes(role);

  useEffect(() => {
    checkAuth();
  }, [checkAuth]);

  if (isLoading) return null;

  return (
    <AuthContext.Provider
      value={{ isAuth, isLoading, userId, roles, checkAuth, logout, hasRole }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
