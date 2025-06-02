import { useEffect, useState } from "react";
import { API } from "../services/api";
import { useAuth } from "../context/AuthContext";

export function useCurrentUserId() {
  const [userId, setUserId] = useState<string | null>(null);
  const { isAuth } = useAuth();

  useEffect(() => {
    if (!isAuth) {
      setUserId(null);
      return;
    }

    API.get("/user/me")
      .then((res) => setUserId(res.data.userId))
      .catch(() => setUserId(null));
  }, [isAuth]);

  return userId;
}
