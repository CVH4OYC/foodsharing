// components/chat/MessageInput.tsx
import { FC, ChangeEvent, FormEvent } from "react";
import { FiSend, FiPaperclip, FiCamera, FiX } from "react-icons/fi";

interface Props {
  tempMessage: string;
  setTempMessage: (value: string) => void;
  image: File | null;
  setImage: (file: File | null) => void;
  file: File | null;
  setFile: (file: File | null) => void;
  onSend: () => void;
  sending: boolean;
}

const MessageInput: FC<Props> = ({
  tempMessage,
  setTempMessage,
  image,
  setImage,
  file,
  setFile,
  onSend,
  sending,
}) => {
  const handleTextChange = (e: ChangeEvent<HTMLInputElement>) => {
    setTempMessage(e.target.value);
  };

  const handleImageChange = (e: ChangeEvent<HTMLInputElement>) => {
    setImage(e.target.files?.[0] || null);
  };

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    setFile(e.target.files?.[0] || null);
  };

  const handleRemoveImage = () => {
    setImage(null);
  };

  const handleRemoveFile = () => {
    setFile(null);
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    if (!sending) {
      onSend();
    }
  };

  return (
    <>
      {/* Превью изображения и файла */}
      {(image || file) && (
        <div className="flex gap-4 mb-2 items-center">
          {image && (
            <div className="relative w-20 h-20">
              <img
                src={URL.createObjectURL(image)}
                alt="preview"
                className="w-20 h-20 object-cover rounded-lg border"
              />
              <button
                onClick={handleRemoveImage}
                className="absolute -top-2 -right-2 bg-white rounded-full shadow p-1 hover:bg-red-100"
              >
                <FiX size={16} />
              </button>
            </div>
          )}
          {file && (
            <div className="relative flex items-center gap-2 px-3 py-2 bg-gray-100 rounded-lg border max-w-xs">
              <FiPaperclip size={20} className="text-gray-500" />
              <div className="text-sm truncate">{file.name}</div>
              <button
                onClick={handleRemoveFile}
                className="absolute -top-2 -right-2 bg-white rounded-full shadow p-1 hover:bg-red-100"
              >
                <FiX size={16} />
              </button>
            </div>
          )}
        </div>
      )}

      {/* Форма ввода */}
      <form onSubmit={handleSubmit} className="flex gap-2 items-center">
        <label className="cursor-pointer text-primary">
          <FiCamera size={20} />
          <input
            type="file"
            onChange={handleImageChange}
            className="hidden"
            accept="image/*"
          />
        </label>
        <label className="cursor-pointer text-primary">
          <FiPaperclip size={20} />
          <input
            type="file"
            onChange={handleFileChange}
            className="hidden"
          />
        </label>
        <input
          type="text"
          placeholder="Введите сообщение..."
          value={tempMessage}
          onChange={handleTextChange}
          className="flex-1 border rounded-xl px-4 py-2 outline-none"
          disabled={sending}
        />

        <button
          type="submit"
          className="text-primary hover:text-green-600"
          disabled={sending}
        >
          <FiSend size={24} />
        </button>
      </form>
    </>
  );
};

export default MessageInput;
