// components/chat/AvatarOrInitials.tsx
import { FC } from "react";

interface Props {
  name?: string;
  imageUrl?: string | null;
  size?: number; // размер в пикселях (ширина и высота)
}

const AvatarOrInitials: FC<Props> = ({ name, imageUrl, size = 40 }) => {
  const dimension = `${size}px`;

  if (imageUrl) {
    return (
      <img
        src={imageUrl}
        alt="avatar"
        className="rounded-full object-cover"
        style={{ width: dimension, height: dimension }}
      />
    );
  }

  const initial = name?.[0]?.toUpperCase() || "?";

  return (
    <div
      className="rounded-full bg-gray-300 flex items-center justify-center text-white font-bold"
      style={{ width: dimension, height: dimension }}
    >
      {initial}
    </div>
  );
};

export default AvatarOrInitials;
