import { FC } from "react";

interface AdCardProps {
  title: string;
  description: string;
  image: string;
  categoryColor?: string;
  categoryName?: string;
  author?: string;
  authorImage?: string;
  date?: string;
  location?: string;
}

const AdCard: FC<AdCardProps> = ({
  title,
  description,
  image,
  categoryColor = "#ccc",
  categoryName,
  author,
  authorImage,
  date,
  location,
}) => {
  return (
    <div className="bg-white rounded-2xl shadow hover:shadow-md transition-all overflow-hidden">
      <div className="relative">
        <img src={image} alt={title} className="w-full h-40 object-cover" />
        {categoryName && (
          <span
            className="absolute top-3 left-3 text-xs font-medium px-3 py-1 rounded-full"
            style={{ backgroundColor: categoryColor, color: "#fff" }}
          >
            {categoryName}
          </span>
        )}
      </div>

      <div className="p-4 space-y-2">
        <h3 className="font-semibold text-base truncate">{title}</h3>
        <p className="text-sm text-gray-600 line-clamp-2">
          {description}
        </p>

        {location && (
          <p className="text-xs text-gray-400 truncate">{location}</p>
        )}

        <div className="flex items-center justify-between pt-2 text-xs text-gray-500">
          <div className="flex items-center gap-2">
            {authorImage && (
              <img
                src={authorImage}
                alt={author}
                className="w-6 h-6 rounded-full object-cover"
              />
            )}
            <span className="truncate">{author}</span>
          </div>
          <span>{date}</span>
        </div>
      </div>
    </div>
  );
};

export default AdCard;
