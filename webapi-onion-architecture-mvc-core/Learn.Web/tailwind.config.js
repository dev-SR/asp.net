/** @type {import('tailwindcss').Config} */
module.exports = {
	content: [
		'./Views/**/*.cshtml',
		// './Pages/**/*.cshtml',
		// './**/*.{razor,html,cshtml}',
		'./node_modules/flowbite/**/*.js'
	],
	theme: {
		extend: {}
	},
	plugins: [require('flowbite/plugin')]
};
