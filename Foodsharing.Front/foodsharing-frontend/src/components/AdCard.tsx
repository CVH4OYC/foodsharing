import { FC } from "react";
import { AdCardProps } from "../types/ads";
import { useNavigate } from "react-router-dom";
import { StaticAPI } from "../services/api";

const AdCard: FC<AdCardProps> = ({
  announcementId,
  title,
  description,
  image,
  categoryColor = "#4CAF50",
  category,
  date,
  address,
  owner,
}) => {
  const navigate = useNavigate();

  const location = [address?.city, address?.street, address?.house]
    .filter(Boolean)
    .join(", ");

  return (
    <div
      onClick={() => navigate(`/ads/${announcementId}`)}
      className="cursor-pointer bg-white rounded-2xl shadow-md hover:shadow-lg transition-all overflow-hidden"
    >
      <div className="relative">
      <img
          src={image}
          alt={title}
          className="w-full h-48 object-cover"
          onError={(e) => {
            const img = e.target as HTMLImageElement;
            if (img.dataset.fallback !== "true") {
              img.src = "/placeholder-image.jpg";
              img.dataset.fallback = "true";
            }
          }}
        />
        {category?.name && (
          <span
            className="absolute top-3 left-3 text-xs font-medium px-3 py-1 rounded-full backdrop-blur-sm shadow-sm"
            style={{
              backgroundColor: `${categoryColor}CC`,
              color: "#000000",
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
        {owner && (
          <div
            className="flex items-center gap-2"
            onClick={(e) => {
              e.stopPropagation();
              if (owner.link) navigate(owner.link);
            }}
          >
            {owner.image ? (
              <img
                src={owner.image}
                alt={owner.name}
                className="w-8 h-8 rounded-full object-cover border-2 border-white shadow-sm"
                onError={(e) => {
                  (e.target as HTMLImageElement).src = `${StaticAPI.defaults.baseURL}/default-avatar.png`;
                }}
              />
            ) : (
              <div className="w-8 h-8 rounded-full bg-gray-200 flex items-center justify-center">
                <span className="text-sm text-gray-600 font-medium">
                  {owner.name?.[0]?.toUpperCase() || "U"}
                </span>
              </div>
            )}
            <span className="text-sm text-primary hover:underline truncate">
              {owner.name}
            </span>
          </div>
        )}

          <span className="text-sm text-gray-500">{date}</span>
        </div>
      </div>
    </div>
  );
};

export default AdCard;
