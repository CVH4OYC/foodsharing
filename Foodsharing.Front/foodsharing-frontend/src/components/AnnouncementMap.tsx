import { useEffect, useState } from "react";
import {
  MapContainer,
  TileLayer,
  Marker,
  Popup,
  useMap,
} from "react-leaflet";
import MarkerClusterGroup from "react-leaflet-cluster";
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import { Announcement } from "../types/ads";
import { StaticAPI } from "../services/api";

interface Props {
  announcements: Announcement[];
}

const AnnouncementMap = ({ announcements }: Props) => {
  const [position, setPosition] = useState<[number, number] | null>(null);

  useEffect(() => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (pos) => {
          setPosition([pos.coords.latitude, pos.coords.longitude]);
        },
        () => {
          setPosition([55.751244, 37.618423]); // fallback — Москва
        }
      );
    } else {
      setPosition([55.751244, 37.618423]); // fallback — Москва
    }
  }, []);

  if (!position) {
    return <div className="text-center py-10 text-gray-500">Получаем ваше местоположение…</div>;
  }

  return (
    <div className="relative">
      <MapContainer
        center={position}
        zoom={11}
        scrollWheelZoom
        className="h-[70vh] w-full rounded-xl overflow-hidden z-0"
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />

        <MarkerClusterGroup chunkedLoading>
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
        </MarkerClusterGroup>

        <MyLocationButton coords={position} />
      </MapContainer>
    </div>
  );
};

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
    map.flyTo([lat, lon], 15, { animate: true, duration: 0.5 });
  };

  return (
    <Marker
      position={[lat, lon]}
      eventHandlers={{ click: handleClick }}
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

function MyLocationButton({ coords }: { coords: [number, number] }) {
  const map = useMap();

  const handleFly = () => {
    map.flyTo(coords, 13, { duration: 0.5 });
  };

  return (
    <button
      onClick={handleFly}
      className="absolute z-[1000] bottom-4 right-4 bg-white text-black shadow-md px-3 py-2 rounded-lg text-sm hover:bg-gray-100 transition"
    >
      Найти меня
    </button>
  );
}

export default AnnouncementMap;
