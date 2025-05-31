import { useEffect, useState } from "react";

interface Suggestion {
  display_name: string;
  address: {
    region?: string;
    state?: string;
    city?: string;
    town?: string;
    village?: string;
    road?: string;
    house_number?: string;
  };
}

export function AddressAutocomplete({
  onSelect,
  showMessage,
}: {
  onSelect: (data: {
    region: string;
    city: string;
    street: string;
    house: string;
  }) => void;
  showMessage?: (msg: string, type: "success" | "error") => void;
}) {
  const [query, setQuery] = useState("");
  const [results, setResults] = useState<Suggestion[]>([]);
  const [selectedDisplayName, setSelectedDisplayName] = useState("");
  const [isAddressSelected, setIsAddressSelected] = useState(false);

  useEffect(() => {
    if (query.length < 3 || isAddressSelected) return;

    const controller = new AbortController();
    const delay = setTimeout(() => {
      fetch(
        `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(
          query
        )}&addressdetails=1&limit=5`,
        {
          headers: { "User-Agent": "foodsharing-client" },
          signal: controller.signal,
        }
      )
        .then((res) => res.json())
        .then(setResults)
        .catch(() => {});
    }, 400);

    return () => {
      controller.abort();
      clearTimeout(delay);
    };
  }, [query, isAddressSelected]);

  const handleSelect = (res: Suggestion) => {
    const a = res.address;

    const region = a.state || a.region || "";
    const city = a.city || a.town || a.village || "";
    const street = a.road || "";
    const house = a.house_number || "";

    if (!street || !house) {
      showMessage?.("Пожалуйста, выберите адрес с улицей и номером дома", "error");
      return;
    }

    setSelectedDisplayName(res.display_name);
    setQuery(res.display_name);
    setResults([]);
    setIsAddressSelected(true);

    onSelect({ region, city, street, house });
  };

  return (
    <div className="space-y-2">
      <label className="block text-sm font-medium text-gray-700">
        Адрес (с подсказкой)
      </label>
      <input
        type="text"
        placeholder="Начните вводить адрес..."
        value={query}
        onChange={(e) => {
          setQuery(e.target.value);
          setSelectedDisplayName("");
          setIsAddressSelected(false);
        }}
        className="w-full border rounded-lg px-4 py-2"
      />
      {results.length > 0 && (
        <ul className="border mt-1 rounded bg-white max-h-60 overflow-y-auto z-50 relative shadow-md">
          {results.map((res, i) => (
            <li
              key={i}
              onClick={() => handleSelect(res)}
              className="cursor-pointer px-4 py-2 hover:bg-gray-100 text-sm"
            >
              {res.display_name}
            </li>
          ))}
        </ul>
      )}
      {selectedDisplayName && (
        <div className="text-sm text-gray-600 italic">
          Выбранный адрес: {selectedDisplayName}
        </div>
      )}
    </div>
  );
}
