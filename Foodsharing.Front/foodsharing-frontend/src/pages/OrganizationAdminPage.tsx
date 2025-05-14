import { Outlet, NavLink, useParams } from "react-router-dom";

const OrganizationAdminPage = () => {
  const { orgId } = useParams();

  return (
    <div className="flex min-h-screen bg-white py-6 px-4 md:px-0 gap-6">
      {/* Боковое меню */}
      <aside className="w-64 bg-white shadow-md p-6 hidden md:block rounded-xl">
        <h2 className="text-xl font-bold mb-6">Организация</h2>
        <ul className="space-y-3">
          <li>
            <NavLink
              to={`/admin/organizations/${orgId}/announcements`}
              className={({ isActive }) =>
                `block text-left w-full ${isActive ? "text-primary font-semibold" : "text-gray-700"}`
              }
            >
              Объявления
            </NavLink>
          </li>
          <li>
            <NavLink
              to={`/admin/organizations/${orgId}/info`}
              className={({ isActive }) =>
                `block text-left w-full ${isActive ? "text-primary font-semibold" : "text-gray-700"}`
              }
            >
              Информация
            </NavLink>
          </li>
          <li>
            <NavLink
              to={`/admin/organizations/${orgId}/representatives`}
              className={({ isActive }) =>
                `block text-left w-full ${isActive ? "text-primary font-semibold" : "text-gray-700"}`
              }
            >
              Представители
            </NavLink>
          </li>
        </ul>
      </aside>

      {/* Контент секции */}
      <main className="flex-1">
        <Outlet />
      </main>
    </div>
  );
};

export default OrganizationAdminPage;
