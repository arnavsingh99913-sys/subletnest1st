# SubletNest Backend

Fullstack Web API built on **ASP.NET Core (.NET 8)** and **Entity Framework Core (SQLite)** for spare room sublets, leaser authentication, image storage, and audit trail logging.

## Structure

```
backend/
├── Controllers/
│   ├── AuthController.cs        # Leaser registration, login & admin verification
│   ├── InquiriesController.cs   # Prospective roommate inquiry handling
│   ├── ListingsController.cs    # Sublet listings management, search & filters
│   └── UploadController.cs      # Live camera snapshots & image file uploads
├── Data/
│   ├── DbInitializer.cs         # Database schema creation & seeding
│   └── SubletDbContext.cs       # EF Core SQLite DbContext
├── DTOs/
│   ├── AuthDtos.cs
│   ├── ListingDtos.cs
│   └── UploadDtos.cs
├── Models/
│   ├── Listing.cs
│   ├── ListingAudit.cs
│   ├── RoomInquiry.cs
│   └── User.cs
├── Properties/
│   └── launchSettings.json
├── Program.cs                   # App configuration, DI, CORS & WebRootPath resolution
├── SubletNest.csproj            # .NET 8 Project file
└── subletnest.db                # SQLite Database file
```

## Running the Backend

From this directory (`backend`):
```bash
dotnet run
```
The server will start at `http://localhost:5000` (and `http://localhost:5170`), automatically host the `../frontend` web app, and provide all REST API endpoints under `/api/...`.

