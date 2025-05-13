// src/pages/AccessDenied.tsx
const AccessDenied = () => {
    return (
      <div className="flex flex-col items-center justify-center h-screen text-center p-4">
        <h1 className="text-3xl font-bold text-red-600 mb-4">403</h1>
        <p className="text-lg text-gray-700">У вас нет доступа к этой странице.</p>
      </div>
    );
  };
  
  export default AccessDenied;
  