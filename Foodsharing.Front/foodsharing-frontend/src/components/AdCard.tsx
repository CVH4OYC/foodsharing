import { FC } from "react";
import { AdCardProps } from '../types/ads';
import { Link } from "react-router-dom";

const AdCard: FC<AdCardProps> = ({
  announcementId,
  title,
  description,
  image,
  categoryColor = "#4CAF50",
  category,
  user,
  date,
  address,
}) => {
  const location = [
    address?.city,
    address?.street,
    address?.house
  ].filter(Boolean).join(", ");

  return (
    <Link to={`/ads/${announcementId}`} className="block">
      <div className="bg-white rounded-2xl shadow-md hover:shadow-lg transition-all overflow-hidden">
        <div className="relative">
          <img
            src={image}
            alt={title}
            className="w-full h-48 object-cover"
            onError={(e) => {
              (e.target as HTMLImageElement).src = "/placeholder-image.jpg";
            }}
          />
          {category?.name && (
            <span
              className="absolute top-3 left-3 text-xs font-medium px-3 py-1 rounded-full backdrop-blur-sm shadow-sm"
              style={{
                backgroundColor: `${categoryColor}CC`,
                color: "#ffffff",
              }}
            >
              {category.name}
            </span>
          )}
        </div>

        <div className="p-4 space-y-3">
          <h3 className="font-semibold text-lg truncate">{title}</h3>
          <p className="text-gray-600 text-sm line-clamp-3">{description}</p>

          {location && (
            <div className="flex items-center text-gray-500 text-sm">
              <svg
                className="w-4 h-4 mr-1"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"
                />
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"
                />
              </svg>
              <span className="truncate">{location}</span>
            </div>
          )}

          <div className="flex items-center justify-between pt-3 border-t border-gray-100">
            <div className="flex items-center gap-2">
              {user?.image ? (
                <img
                  src={user.image}
                  alt={user.userName}
                  className="w-8 h-8 rounded-full object-cover border-2 border-white shadow-sm"
                  onError={(e) => {
                    (e.target as HTMLImageElement).src = "/default-avatar.png";
                  }}
                />
              ) : (
                <div className="w-8 h-8 rounded-full bg-gray-200 flex items-center justify-center">
                  <span className="text-sm text-gray-600 font-medium">
                    {user?.userName?.[0]?.toUpperCase() || 'U'}
                  </span>
                </div>
              )}
              <Link
                to={`/profile/user/${user?.userId}`}
                className="text-sm text-primary hover:underline truncate"
              >
                {user?.userName}
              </Link>
            </div>
            <span className="text-sm text-gray-500">{date}</span>
          </div>
        </div>
      </div>
    </Link>
  );
};

export default AdCard;