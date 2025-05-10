import { useEffect, useState } from "react";
import { API } from "../services/api";
import { useLocation, useNavigate } from "react-router-dom";

const PartnershipForm = () => {
  const [forms, setForms] = useState<{ id: string; organizationFormFullName: string }[]>([]);
  const [orgName, setOrgName] = useState("");
  const [orgPhone, setOrgPhone] = useState("");
  const [orgEmail, setOrgEmail] = useState("");
  const [orgWebsite, setOrgWebsite] = useState("");
  const [orgDescription, setOrgDescription] = useState("");
  const [orgFormId, setOrgFormId] = useState("");
  const [orgImage, setOrgImage] = useState<File | null>(null);
  const [region, setRegion] = useState("");
  const [city, setCity] = useState("");
  const [street, setStreet] = useState("");
  const [house, setHouse] = useState("");

  const [repPhone, setRepPhone] = useState("");
  const [repEmail, setRepEmail] = useState("");
  const [useSameContacts, setUseSameContacts] = useState(false);

  const [message, setMessage] = useState<string | null>(null);
  const [messageType, setMessageType] = useState<"success" | "error">("success");

  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    API.get("/organization/forms").then((res) => {
      setForms(res.data);
    });
  }, []);

  useEffect(() => {
    if (useSameContacts) {
      setRepPhone(orgPhone);
      setRepEmail(orgEmail);
    }
  }, [useSameContacts, orgPhone, orgEmail]);

  const resetForm = () => {
    setOrgName("");
    setOrgPhone("");
    setOrgEmail("");
    setOrgWebsite("");
    setOrgDescription("");
    setOrgFormId("");
    setOrgImage(null);
    setRegion("");
    setCity("");
    setStreet("");
    setHouse("");
    setRepPhone("");
    setRepEmail("");
    setUseSameContacts(false);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!orgImage) {
      setMessage("Пожалуйста, загрузите логотип организации");
      setMessageType("error");
      setTimeout(() => setMessage(null), 4000);
      return;
    }

    const formData = new FormData();
    formData.append("Organization.Name", orgName);
    formData.append("Organization.Phone", orgPhone);
    formData.append("Organization.Email", orgEmail);
    formData.append("Organization.Website", orgWebsite);
    formData.append("Organization.Description", orgDescription);
    formData.append("Organization.OrganizationFormId", orgFormId);
    formData.append("Organization.Address.Region", region);
    formData.append("Organization.Address.City", city);
    formData.append("Organization.Address.Street", street);
    formData.append("Organization.Address.House", house);
    formData.append("Organization.ImageFile", orgImage);
    formData.append("Phone", repPhone);
    formData.append("Email", repEmail);

    try {
      await API.post("/parthnership/createApplication", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      setMessage("Заявка успешно отправлена!");
      setMessageType("success");
      resetForm();
      navigate(location.pathname); // остаёмся на той же странице
    } catch (err) {
      setMessage("Ошибка при отправке заявки");
      setMessageType("error");
      console.error(err);
    } finally {
      setTimeout(() => setMessage(null), 4000);
    }
  };

  return (
    <div className="max-w-3xl mx-auto bg-white rounded-xl shadow-xl p-6 mt-10">
      {message && (
        <div
          className={`fixed top-4 left-1/2 transform -translate-x-1/2 px-6 py-3 rounded-xl shadow-md z-50 animate-fade-in ${
            messageType === "success" ? "bg-green-500 text-white" : "bg-red-500 text-white"
          }`}
        >
          {message}
        </div>
      )}

      <h2 className="text-2xl font-semibold mb-6">Заявка на партнёрство</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <input type="text" placeholder="Название организации" className="w-full border px-4 py-2 rounded-xl" value={orgName} onChange={(e) => setOrgName(e.target.value)} required />
        <input type="text" placeholder="Телефон организации" className="w-full border px-4 py-2 rounded-xl" value={orgPhone} onChange={(e) => setOrgPhone(e.target.value)} required />
        <input type="email" placeholder="Email организации" className="w-full border px-4 py-2 rounded-xl" value={orgEmail} onChange={(e) => setOrgEmail(e.target.value)} required />
        <input type="text" placeholder="Сайт (необязательно)" className="w-full border px-4 py-2 rounded-xl" value={orgWebsite} onChange={(e) => setOrgWebsite(e.target.value)} />
        <textarea placeholder="Описание (необязательно)" className="w-full border px-4 py-2 rounded-xl" value={orgDescription} onChange={(e) => setOrgDescription(e.target.value)} maxLength={500} />

        <select className="w-full border px-4 py-2 rounded-xl" value={orgFormId} onChange={(e) => setOrgFormId(e.target.value)} required>
          <option value="">Выберите форму собственности организации</option>
          {forms.map((form) => (
            <option key={form.id} value={form.id}>
              {form.organizationFormFullName}
            </option>
          ))}
        </select>

        <div className="grid grid-cols-2 gap-2">
          <input type="text" placeholder="Регион" className="border px-4 py-2 rounded-xl" value={region} onChange={(e) => setRegion(e.target.value)} />
          <input type="text" placeholder="Город" className="border px-4 py-2 rounded-xl" value={city} onChange={(e) => setCity(e.target.value)} />
        </div>
        <div className="grid grid-cols-2 gap-2">
          <input type="text" placeholder="Улица" className="border px-4 py-2 rounded-xl" value={street} onChange={(e) => setStreet(e.target.value)} />
          <input type="text" placeholder="Дом" className="border px-4 py-2 rounded-xl" value={house} onChange={(e) => setHouse(e.target.value)} />
        </div>

        <div className="relative group">
          <label className="block mb-2 text-sm font-medium text-gray-700">Логотип организации</label>
          <div className="flex items-center justify-center w-full">
            <label className="flex flex-col items-center justify-center w-full h-32 border-2 border-dashed rounded-xl cursor-pointer hover:border-primary transition-colors">
              {orgImage ? (
                <img src={URL.createObjectURL(orgImage)} alt="Preview" className="w-full h-full object-cover rounded-xl" />
              ) : (
                <>
                  <svg className="w-8 h-8 text-gray-400 mb-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                  <span className="text-sm text-gray-500">Нажмите для загрузки</span>
                </>
              )}
              <input type="file" accept="image/*" onChange={(e) => { const file = e.target.files?.[0]; if (file) setOrgImage(file); }} className="hidden" />
            </label>
          </div>
        </div>

        <label className="flex items-center gap-2">
          <input type="checkbox" checked={useSameContacts} onChange={() => setUseSameContacts(!useSameContacts)} />
          Использовать те же телефон и email для представителя
        </label>

        {!useSameContacts && (
          <>
            <input type="text" placeholder="Телефон представителя" className="w-full border px-4 py-2 rounded-xl" value={repPhone} onChange={(e) => setRepPhone(e.target.value)} required />
            <input type="email" placeholder="Email представителя" className="w-full border px-4 py-2 rounded-xl" value={repEmail} onChange={(e) => setRepEmail(e.target.value)} required />
          </>
        )}

        <button type="submit" className="w-full bg-primary text-white py-3 px-6 rounded-xl hover:bg-primary-dark transition-colors">
          Отправить заявку
        </button>
      </form>
    </div>
  );
};

export default PartnershipForm;
