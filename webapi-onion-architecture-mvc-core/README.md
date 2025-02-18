# Template MVC Core with Onion Arch.

- [Template MVC Core with Onion Arch.](#template-mvc-core-with-onion-arch)
  - [Setup](#setup)
    - [Create Project](#create-project)
    - [Tailwind CSS](#tailwind-css)
      - [flowbite plugins](#flowbite-plugins)


## Setup

### Create Project

```bash
dotnet new sln -n Learn
dotnet new gitignore

dotnet new mvc --auth Individual -n Learn.Web -o Learn.Web
dotnet new classlib -n Learn.Common -o Learn.Common
dotnet new classlib -n Learn.Entities -o Learn.Entities
dotnet new classlib -n Learn.Repository -o Learn.Repository
dotnet new classlib -n Learn.Services -o Learn.Services

dotnet sln Learn.sln add Learn.Common/
dotnet sln Learn.sln add Learn.Entities/
dotnet sln Learn.sln add Learn.Repository/
dotnet sln Learn.sln add Learn.Services/
dotnet sln Learn.sln add Learn.Web/

// add reference 
dotnet add Learn.Entities/ reference Learn.Common/

dotnet add Learn.Repository/ reference Learn.Common/
dotnet add Learn.Repository/ reference Learn.Entities/

dotnet add Learn.Services/ reference Learn.Common/
dotnet add Learn.Services/ reference Learn.Repository/
dotnet add Learn.Services/ reference Learn.Entities/

dotnet add Learn.Web/ reference Learn.Services/

dotnet build Learn.sln
```

### Tailwind CSS

```bash
pnpm add tailwindcss
npx tailwindcss init 
```

`tailwind.config.js`

```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
	content: [
		'./Views/**/*.cshtml',
		// './Pages/**/*.cshtml',
		// './**/*.{razor,html,cshtml}',
	],
	theme: {
		extend: {}
	},
	plugins: []
};

```


`package.json`

```json
  "scripts": {
    "ts:build": "npx tailwindcss -i ./wwwroot/css/tailwind.in.css -o ./wwwroot/css/tailwind.out.css --minify",
    "ts:watch": "npx tailwindcss -i ./wwwroot/css/tailwind.in.css -o ./wwwroot/css/tailwind.out.css --minify --watch"
  },
```

`.\wwwroot\css\tailwind.in.css`

```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

Add a CSS reference to the host file in the `_Layout.cshtml`:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <!-- ... -->
    <link rel="stylesheet" href="~/css/tailwind.out.css" asp-append-version="true" />
</head>
<!--... -->
```

#### flowbite plugins

First, you need to install Flowbite via NPM: `pnpm add flowbite`

Require Flowbite in the Tailwind configuration file as a plugin:

```javascript
module.exports = {
  // other options
  plugins: [
    require('flowbite/plugin')
  ],
}
```

Add the Flowbite source files to the content module to start applying utility classes from the interactive UI components such as the dropdowns, modals, and navbars:

```javascript
module.exports = {
  content: [
    // other files...
    "./node_modules/flowbite/**/*.js"
  ],
...
}
```

Add a script tag with this path before the end of the body tag in the `_Layout.cshtml` page:

```html
<!-- ... -->
    <script src="https://cdn.jsdelivr.net/npm/flowbite@2.5.2/dist/flowbite.min.js"></script>
  </body>
</html>
```

You have successfully installed Flowbite and can start using the components to build your  project.