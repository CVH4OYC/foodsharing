import PartnershipForm from "../components/PartnershipForm";

const BusinessPage = () => {
  return (
    <div className="max-w-3xl mx-auto py-10 px-4">
      <h1 className="text-3xl font-bold mb-6">Станьте нашим партнёром</h1>
      <p className="mb-8 text-gray-600">
        Заполните форму, чтобы отправить заявку на партнёрство. Мы свяжемся с вами после рассмотрения.
      </p>
      <PartnershipForm />
    </div>
  );
};

export default BusinessPage;
