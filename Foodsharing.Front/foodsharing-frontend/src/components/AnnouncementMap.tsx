import { useRef } from "react";
import { MapContainer, TileLayer, Marker, Popup, useMap } from "react-leaflet";
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import { Announcement } from "../types/ads";
import { StaticAPI } from "../services/api";

interface Props {
  announcements: Announcement[];
}

const AnnouncementMap = ({ announcements }: Props) => {
  return (
    <MapContainer
      center={[55.751244, 37.618423]}
      zoom={11}
      scrollWheelZoom={true}
      className="h-[70vh] w-full rounded-xl overflow-hidden"
    >
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />

      {announcements
        .filter(
          (a) =>
            a.address &&
            typeof a.address.latitude === "number" &&
            typeof a.address.longitude === "number"
        )
        .map((a) => (
          <AnnouncementMarker key={a.announcementId} announcement={a} />
        ))}
    </MapContainer>
  );
};

// 🔹 Отдельный компонент для маркера с автозумом
function AnnouncementMarker({ announcement }: { announcement: Announcement }) {
  const map = useMap();

  const lat = announcement.address!.latitude!;
  const lon = announcement.address!.longitude!;
  const owner = announcement.user ?? announcement.organization;
  const ownerName = announcement.user
    ? `${announcement.user.firstName || ""} ${announcement.user.lastName || ""}`.trim()
    : announcement.organization?.name ?? "Неизвестно";
  const image = `${StaticAPI.defaults.baseURL}${announcement.image}`;

  const handleClick = () => {
    map.flyTo([lat, lon], 15, {
        animate: true,
        duration: 0.5, 
      });
  };

  return (
    <Marker
      position={[lat, lon]}
      eventHandlers={{
        click: handleClick,
      }}
      icon={L.icon({
        iconUrl: `${StaticAPI.defaults.baseURL}Pictures/Map/marker.png`,
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowUrl: "/marker-shadow.png",
      })}
    >
      <Popup>
        <div className="w-[200px]">
          <img src={image} alt={announcement.title} className="w-full h-24 object-cover rounded mb-2" />
          <h3 className="text-sm font-semibold line-clamp-2 mb-1">{announcement.title}</h3>
          <p className="text-xs text-gray-500 mb-2">{ownerName}</p>
          <a href={`/ads/${announcement.announcementId}`} className="text-primary text-sm underline">
            Посмотреть
          </a>
        </div>
      </Popup>
    </Marker>
  );
}

export default AnnouncementMap;
