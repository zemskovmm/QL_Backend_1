// tailwind.config.js
module.exports = {
    purge: {
        enable: true,
        content: ["./src/**/*.html", "./src/**/*.tsx"],
    },
    darkMode: "media", // or 'media' or 'class'
    theme: {
        extend: {},
    },
    variants: {},
    plugins: [],
};
