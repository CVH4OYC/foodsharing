// components/AnnouncementList.tsx
import React from "react";
import AdCard from "./AdCard";
import { Announcement } from "../types/ads";
import { StaticAPI } from "../services/api";

type AnnouncementListProps = {
  announcements: Announcement[];
};

export default function AnnouncementList({ announcements }: AnnouncementListProps) {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
      {announcements.map((ad) => {
        const owner = ad.user ?? ad.organization;
        const ownerLink = ad.user
          ? `/profile/user/${ad.user.userId}`
          : ad.organization
          ? `/organizations/${ad.organization.id}`
          : null;
        const ownerImage = ad.user?.image
          ? `${StaticAPI.defaults.baseURL}${ad.user.image}`
          : ad.organization?.logoImage
          ? `${StaticAPI.defaults.baseURL}${ad.organization.logoImage}`
          : `${StaticAPI.defaults.baseURL}/default-avatar.png`;
        const ownerName = ad.user
          ? `${ad.user.firstName || ""} ${ad.user.lastName || ""}`.trim()
          : ad.organization?.name ?? "Неизвестно";

        return (
          <AdCard
            key={ad.announcementId}
            {...ad}
            image={`${StaticAPI.defaults.baseURL}${ad.image}`}
            categoryColor={ad.category?.color || "#4CAF50"}
            date={new Date(ad.dateCreation).toLocaleDateString("ru-RU")}
            owner={{ name: ownerName, image: ownerImage, link: ownerLink }}
          />
        );
      })}
    </div>
  );
}
