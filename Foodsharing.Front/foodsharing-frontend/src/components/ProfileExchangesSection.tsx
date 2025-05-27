import { useEffect, useState } from "react";
import { API, StaticAPI } from "../services/api";
import { TransactionDTO } from "../types/transaction";

const ProfileExchangesSection = () => {
  const [filter, setFilter] = useState<"sender" | "recipient">("sender");
  const [transactions, setTransactions] = useState<TransactionDTO[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchTransactions = async () => {
      setLoading(true);
      try {
        const res = await API.get("/Transaction/my", {
          params: { isSender: filter === "sender" },
        });
        setTransactions(res.data);
      } catch (err) {
        console.error("Ошибка при получении обменов", err);
      } finally {
        setLoading(false);
      }
    };

    fetchTransactions();
  }, [filter]);

  const getStatusColor = (status: string) => {
    switch (status) {
      case "Забронировано":
        return "border-primary bg-blue-50";
      case "Отменено":
      case "Завершено":
      default:
        return "bg-white";
    }
  };

  return (
    <div>
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Мои обмены</h1>
        <div className="flex gap-2">
          <button
            className={`px-4 py-2 rounded-xl text-sm ${
              filter === "sender" ? "bg-primary text-white" : "bg-white border"
            }`}
            onClick={() => setFilter("sender")}
          >
            Я отдаю
          </button>
          <button
            className={`px-4 py-2 rounded-xl text-sm ${
              filter === "recipient" ? "bg-primary text-white" : "bg-white border"
            }`}
            onClick={() => setFilter("recipient")}
          >
            Я получаю
          </button>
        </div>
      </div>

      {loading ? (
        <p className="text-center text-gray-500">Загрузка...</p>
      ) : transactions.length === 0 ? (
        <p className="text-center text-gray-500">Нет обменов</p>
      ) : (
        <div className="space-y-4">
          {transactions.map((tr) => (
            <div
              key={tr.id}
              className={`rounded-xl border p-4 flex items-center justify-between ${getStatusColor(tr.status)}`}
            >
              <div className="flex items-center gap-4">
                <div className="w-20 h-20 bg-gray-200 rounded-lg overflow-hidden">
                  {tr.announcement?.image ? (
                    <img
                      src={`${StaticAPI.defaults.baseURL}${tr.announcement.image}`}
                      alt={tr.announcement.title}
                      className="w-full h-full object-cover"
                    />
                  ) : null}
                </div>
                <div>
                  <h3 className="font-semibold">{tr.announcement?.title}</h3>
                  <p className="text-sm text-gray-500">
                    {new Date(tr.transactionDate).toLocaleDateString("ru-RU")}
                  </p>
                  <p className="text-xs mt-1 font-semibold text-gray-600">{tr.status}</p>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ProfileExchangesSection;
