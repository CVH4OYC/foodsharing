import React from "react";

type Props = {
  message: string;
  type: "success" | "error";
};

const NotificationBanner: React.FC<Props> = ({ message, type }) => {
  return (
    <div
      className={`fixed top-4 left-1/2 transform -translate-x-1/2 px-6 py-3 rounded-xl shadow-md z-50 animate-fade-in ${
        type === "success" ? "bg-green-500 text-white" : "bg-red-500 text-white"
      }`}
    >
      {message}
    </div>
  );
};

export default NotificationBanner;