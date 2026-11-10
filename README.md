# SubletNest

SubletNest is a marketplace for spare rooms in shared flats across India, listed together with the furniture the previous tenant is leaving behind.

- **Backend:** ASP.NET Core (.NET 8) Web API with an SQLite database
- **Frontend:** a single HTML page (Tailwind CSS and Lucide icons from a CDN), served by the backend

## Run it on your computer

### 1. Install the .NET 8 SDK

Download it from <https://dotnet.microsoft.com/download/dotnet/8.0>. Check it's installed:

```bash
dotnet --version
```

It should print `8.0.x` or later. You don't need Node.js or a database server.

### 2. Get the code

```bash
git clone https://github.com/arnavsingh99913-sys/subletnest1st.git
cd subletnest1st
```

### 3. Start the app

Run it **from the `backend` folder**:

```bash
cd backend
dotnet run
```

Then open **<http://localhost:5000>**.

The first run takes a minute while .NET downloads packages and builds the app.

### What happens on first run

- An SQLite database `backend/subletnest.db` is created, with 4 sample listings and an administrator account.
- A `frontend/uploads/` folder is created for room photos.

Neither is stored in Git, so every copy of the project starts with its own fresh data.

> **Why run from `backend`?** The database path is relative to the folder you start in. `dotnet run --project backend` from the project root also works, but it creates a second, separate database in the root folder.

### Port 5000 already in use?

On macOS, AirPlay Receiver uses port 5000. Start the app on another port instead:

```bash
dotnet run --urls http://localhost:5080
```

Then open <http://localhost:5080>. The page always talks to the server it was loaded from, so any port works.

## Using the app

1. **Choose a location.** On the first visit you pick a state, a city and, optionally, an area. Use the location pill in the header to change it later.
2. **Browse.** Without an account you can see one matching room. Sign in to see them all.
3. **Create an account** as one of:

   | Account | Can do |
   |---|---|
   | **Buyer**: looking for a room | See all listings, save rooms, send inquiries to sellers, track replies under **My Inquiries** |
   | **Seller**: has a room to let | List rooms with photos and furniture, edit or remove them, accept or decline inquiries under **My Listings** |

An administrator account is created on first run; it can do both, plus open the admin portal (all users and listing history). Its login is set in `backend/Data/DbInitializer.cs`. Change it before you put the app anywhere other people can reach.

## Project layout

```
subletnest1st/
├── backend/                  # .NET 8 Web API (also serves the frontend)
│   ├── Controllers/          # Auth, Listings, Inquiries, Upload
│   ├── Data/                 # EF Core DbContext and first-run seeding
│   ├── DTOs/                 # Request / response models
│   ├── Models/               # Database entities
│   ├── Program.cs            # Startup: database, CORS, static files
│   └── SubletNest.csproj
├── frontend/
│   ├── index.html            # The whole app page
│   ├── onboarding/           # Location picker, sign-in / sign-up, admin portal
│   ├── skiper-gestures/      # Button hover and press effects
│   └── skiper-theme/         # Dark theme and scroll fades
├── index.html                # Redirects to frontend/ if opened directly
└── SubletNest.sln
```

## API

All endpoints are under `/api`:

| Method | Path | Purpose |
|---|---|---|
| GET | `/api/health` | Health check |
| GET | `/api/listings` | List rooms (filters: `search`, `city`, `bhk`, `roomType`, `maxBudget`, `furnishing`, `tenantEmail`) |
| GET / POST / PUT / DELETE | `/api/listings/{id}` | Read, create, update or remove a room |
| POST | `/api/auth/signup` | Create a Buyer or Seller account |
| POST | `/api/auth/login` | Sign in with name, email or phone, plus password |
| POST | `/api/inquiries` | Send an inquiry about a room |
| GET | `/api/inquiries/sent?seekerEmail=` | Inquiries a buyer has sent |
| GET | `/api/inquiries/my-inquiries?tenantEmail=` | Inquiries a seller has received |
| PUT | `/api/inquiries/{id}/status` | Accept or decline an inquiry |
| POST | `/api/upload`, `/api/upload/camera` | Upload a photo file or a camera snapshot |

## Known limitations

This is a demo, not production-ready:

- **No server-side sessions.** The browser remembers who is signed in, and the API doesn't check who is calling. For example, the one-listing limit for guests is applied in the page only.
- **Google sign-in is simulated.** It asks for an email address and doesn't contact Google.
- **The admin password is in the source code** and is reset on every start.
