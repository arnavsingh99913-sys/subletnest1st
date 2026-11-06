# SubletNest Frontend

The full frontend application for **SubletNest** featuring the **Skiper-UI 87 noir satin aesthetic**, **Skiper-UI 43 cursor-responsive button gestures**, interactive location selection across all Indian states/cities, and spare room listing configurator.

## Structure

```
frontend/
├── index.html                 # Main application single-page interface
├── onboarding/                # Location selection cockpit & leaser auth
│   ├── onboarding.css
│   └── onboarding.js
├── skiper-gestures/           # Skiper-UI 43 cursor magnetic pull, 3D tilt & tooltip pill
│   ├── skiper-gestures.css
│   └── skiper-gestures.js
├── skiper-theme/              # Skiper-UI 87 scroll-driven edge fade effect & noir theme
│   └── skiper87.css
├── uploads/                   # Local uploaded room images and camera snapshots
└── package.json
```

## Running the Frontend

### Option 1: Served directly by ASP.NET Core Backend
When the backend runs (`dotnet run` in `../backend`), it automatically detects this `frontend/` folder and serves it at `http://localhost:5000/`.

### Option 2: Standalone Static Server
You can also run the frontend independently using any static web server:
```bash
# Using npx serve
npx serve -l 3000

# Using python
python -m http.server 3000
```
The frontend automatically detects standalone mode and communicates with the backend API running at `http://localhost:5000`.

