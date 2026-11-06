# SubletNest Fullstack Project

**SubletNest** is a fullstack spare room subletting and leftover furniture inventory platform. The project is cleanly structured into two main standalone folders: `frontend` and `backend`.

```
subletdotnet backend/
├── frontend/                  # Full Frontend Web Application
│   ├── index.html             # Single-page application (Tailwind, Lucide, Noir theme)
│   ├── onboarding/            # All-India States/Cities location radar & auth flow
│   ├── skiper-gestures/       # Skiper-UI 43 cursor magnetic pull & floating tooltip
│   ├── skiper-theme/          # Skiper-UI 87 pure CSS scroll-driven edge fade effect
│   ├── uploads/               # Uploaded images and live camera snapshots
│   ├── package.json           # Standalone frontend scripts
│   └── README.md
│
├── backend/                   # Full Backend .NET 8 Web API
│   ├── Controllers/           # Auth, Listings, Inquiries, and Upload controllers
│   ├── Data/                  # SubletDbContext & SQLite Initializer
│   ├── DTOs/                  # Request / Response models
│   ├── Models/                # User, Listing, ListingAudit, RoomInquiry models
│   ├── Properties/            # launchSettings.json (ports 5000 & 5170)
│   ├── Program.cs             # ASP.NET Core startup & frontend auto-detection
│   ├── SubletNest.csproj      # .NET 8 Project file
│   ├── subletnest.db          # SQLite Database
│   └── README.md
│
├── SubletNest.sln             # Root Visual Studio solution file
├── index.html                 # Convenience entrypoint redirecting to frontend/
└── README.md                  # Project overview
```

---

## Getting Started

### 1. Run the Backend & Frontend Together
You can run the backend directly from root or from the `backend/` folder. The backend automatically detects the sibling `frontend/` directory and serves it:

```bash
# From root directory:
dotnet run --project backend

# Or inside backend directory:
cd backend
dotnet run
```
* **Frontend App**: `http://localhost:5000/` (or `http://localhost:5170/`)
* **API Health Check**: `http://localhost:5000/api/health`
* **Swagger/API Endpoints**: `http://localhost:5000/api/listings`

---

### 2. Run the Frontend Standalone (Optional)
The frontend can also be served independently:
```bash
cd frontend
npx serve -l 3000
```
It will automatically connect to the backend running at `http://localhost:5000`.

---

## Features
- **Skiper-UI 87 Aesthetic**: Soft matte noir black palette, satin card depth, and scroll-driven edge fade masks.
- **Skiper-UI 43 Gestures**: Cursor-responsive magnetic tilt, dynamic specular sheen, and floating spring tooltip pill.
- **Location Selector**: All Indian states and cities with skippable area search.
- **Leaser & Admin Auth**: Leaser sign-up/in and Master Admin portal (`arnavsingh99913@gmail.com` / `Arnav$123`).
- **Live Camera & Uploads**: Upload or take live photos saved directly to `frontend/uploads`.
