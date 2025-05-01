/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: '#4CAF50', // Основной зелёный
        footerbg: '#F2FCE2', // Цвет футера
      },
      borderRadius: {
        '2xl': '16px', // Закругления 16px
      },
      height: {
        'screen-dynamic': 'calc(100vh - 100px)', // Динамическая высота
      }
    },
  },
  plugins: [],
}