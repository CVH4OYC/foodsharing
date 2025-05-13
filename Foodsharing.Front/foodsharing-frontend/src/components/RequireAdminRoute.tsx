import React from "react";
import { useAuth } from "../context/AuthContext";
import AccessDenied from "../pages/AccessDenied";

const RequireAdminRoute = ({ children }: { children: React.ReactElement }) => {
  const { isAuth, hasRole } = useAuth();

  if (!isAuth) return <AccessDenied />; // Можно показать лоадер
  if (!hasRole("Admin")) return <AccessDenied />;

  return children;
};

export default RequireAdminRoute;
