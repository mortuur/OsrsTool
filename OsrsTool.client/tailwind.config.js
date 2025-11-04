/** @type {import('tailwindcss').Config} */
export default {
    // include both lowercase `src` and capitalized `Src` to match this repo
    content: [
        './index.html',
        './src/**/*.{js,ts,jsx,tsx}',
        './Src/**/*.{js,ts,jsx,tsx}',
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/typography'),
    ],
};
