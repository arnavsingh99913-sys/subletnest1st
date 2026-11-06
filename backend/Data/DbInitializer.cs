using Microsoft.EntityFrameworkCore;
using SubletNest.Models;

namespace SubletNest.Data;

public static class DbInitializer
{
    public static void Initialize(SubletDbContext context)
    {
        context.Database.EnsureCreated();

        // Ensure Users table and indexes exist on existing SQLite databases
        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""Users"" (
                ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Users"" PRIMARY KEY AUTOINCREMENT,
                ""Name"" TEXT NOT NULL,
                ""Contact"" TEXT NOT NULL,
                ""Email"" TEXT NOT NULL,
                ""PasswordHash"" TEXT NOT NULL,
                ""Role"" TEXT NOT NULL,
                ""Initials"" TEXT NOT NULL,
                ""CreatedAt"" TEXT NOT NULL
            );
        ");
        context.Database.ExecuteSqlRaw(@"CREATE UNIQUE INDEX IF NOT EXISTS ""IX_Users_Email"" ON ""Users"" (""Email"");");
        context.Database.ExecuteSqlRaw(@"CREATE INDEX IF NOT EXISTS ""IX_Users_Contact"" ON ""Users"" (""Contact"");");
        context.Database.ExecuteSqlRaw(@"CREATE INDEX IF NOT EXISTS ""IX_Users_Name"" ON ""Users"" (""Name"");");

        // Ensure ListingAudits table exists for Admin DB removed rooms audit
        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""ListingAudits"" (
                ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_ListingAudits"" PRIMARY KEY AUTOINCREMENT,
                ""ListingId"" INTEGER NOT NULL,
                ""Title"" TEXT NOT NULL,
                ""Location"" TEXT NOT NULL,
                ""Rent"" TEXT NOT NULL,
                ""TenantName"" TEXT NOT NULL,
                ""TenantEmail"" TEXT NOT NULL,
                ""TenantPhone"" TEXT NULL,
                ""ListedAt"" TEXT NOT NULL,
                ""RemovedAt"" TEXT NOT NULL,
                ""Action"" TEXT NOT NULL
            );
        ");

        // Ensure Admin user arnavsingh99913@gmail.com with password Arnav$123 exists
        var adminEmails = new[] { "arnavsingh99913@gmail.com", "arnavsingh9913@gmail.com" };
        foreach (var adminEmail in adminEmails)
        {
            var adminUser = context.Users.FirstOrDefault(u => u.Email.ToLower() == adminEmail.ToLower());
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Name = "Arnav Singh",
                    Contact = "+91 99913 00000",
                    Email = adminEmail,
                    PasswordHash = Controllers.AuthController.HashPassword("Arnav$123"),
                    Role = "Administrator",
                    Initials = "AS",
                    CreatedAt = DateTime.UtcNow
                };
                context.Users.Add(adminUser);
            }
            else
            {
                adminUser.Name = "Arnav Singh";
                adminUser.PasswordHash = Controllers.AuthController.HashPassword("Arnav$123");
                adminUser.Role = "Administrator";
            }
        }
        context.SaveChanges();

        if (context.Listings.Any())
        {
            return; // DB already seeded
        }

        var listings = new List<Listing>
        {
            new Listing
            {
                Title = "Master Bedroom in 2 BHK with 2 Chairs & Study Desk",
                TotalBhk = "2 BHK",
                Offering = "1 Master Bed with Attached Bath",
                Location = "Indiranagar, Bangalore",
                Rent = 16500,
                Deposit = 35000,
                AvailableDate = "Immediate Move-in",
                Bills = "Split 50/50 evenly",
                Condition = "Gently Used - Great",
                TenantName = "Rohan K.",
                TenantInitial = "RK",
                TenantEmail = "rohan@example.com",
                TenantRole = "Primary Tenant",
                Images = new List<ListingImage>
                {
                    new ListingImage { Url = "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?auto=format&fit=crop&w=800&q=80", IsCover = true, Source = "stock" },
                    new ListingImage { Url = "https://images.unsplash.com/photo-1598928506311-c55ded91a20c?auto=format&fit=crop&w=800&q=80", IsCover = false, Source = "stock" }
                },
                Inventory = new List<InventoryItem>
                {
                    new InventoryItem { Name = "Bed", Detail = "1 Queen Bed with Ortho Mattress", Quantity = 1 },
                    new InventoryItem { Name = "Chairs", Detail = "2 Ergonomic Study Chairs", Quantity = 2 },
                    new InventoryItem { Name = "Desk", Detail = "1 Teak Wood Study Table", Quantity = 1 },
                    new InventoryItem { Name = "Wardrobe", Detail = "1 3-Door Wardrobe", Quantity = 1 },
                    new InventoryItem { Name = "AC", Detail = "1.5 Ton Split AC", Quantity = 1 }
                },
                SharedAmenities = new List<SharedAmenity>
                {
                    new SharedAmenity { Name = "3-Seater Sofa" },
                    new SharedAmenity { Name = "Double Door Fridge" },
                    new SharedAmenity { Name = "Washing Machine" },
                    new SharedAmenity { Name = "RO Water Filter" },
                    new SharedAmenity { Name = "Microwave" }
                }
            },
            new Listing
            {
                Title = "Sunny Private Room in 3 BHK (Previous tenant left entire study setup)",
                TotalBhk = "3 BHK",
                Offering = "1 Private Bedroom",
                Location = "Bandra West, Mumbai",
                Rent = 22000,
                Deposit = 50000,
                AvailableDate = "1st Next Month",
                Bills = "Included in Rent",
                Condition = "Brand New",
                TenantName = "Priya S.",
                TenantInitial = "PS",
                TenantEmail = "priya@example.com",
                TenantRole = "Primary Tenant",
                Images = new List<ListingImage>
                {
                    new ListingImage { Url = "https://images.unsplash.com/photo-1598928506311-c55ded91a20c?auto=format&fit=crop&w=800&q=80", IsCover = true, Source = "stock" },
                    new ListingImage { Url = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?auto=format&fit=crop&w=800&q=80", IsCover = false, Source = "stock" }
                },
                Inventory = new List<InventoryItem>
                {
                    new InventoryItem { Name = "Bed", Detail = "1 Single Bed with Storage", Quantity = 1 },
                    new InventoryItem { Name = "Chairs", Detail = "1 Reclining Office Chair", Quantity = 1 },
                    new InventoryItem { Name = "Desk", Detail = "1 Corner Work Desk", Quantity = 1 },
                    new InventoryItem { Name = "AC", Detail = "1 Split AC installed", Quantity = 1 }
                },
                SharedAmenities = new List<SharedAmenity>
                {
                    new SharedAmenity { Name = "Large Couch" },
                    new SharedAmenity { Name = "Dining Table (6-seater)" },
                    new SharedAmenity { Name = "Washing Machine" },
                    new SharedAmenity { Name = "Microwave Oven" }
                }
            },
            new Listing
            {
                Title = "Cozy Room in 2.5 BHK Penthouse with Balcony & Leftover Chairs",
                TotalBhk = "2 BHK",
                Offering = "Entire 1 BHK Portion",
                Location = "Cyber City, Gurgaon",
                Rent = 19000,
                Deposit = 40000,
                AvailableDate = "Immediate Move-in",
                Bills = "Split 50/50 evenly",
                Condition = "Well Maintained",
                TenantName = "Aman V.",
                TenantInitial = "AV",
                TenantEmail = "amanv@example.com",
                TenantRole = "Primary Tenant",
                Images = new List<ListingImage>
                {
                    new ListingImage { Url = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?auto=format&fit=crop&w=800&q=80", IsCover = true, Source = "stock" }
                },
                Inventory = new List<InventoryItem>
                {
                    new InventoryItem { Name = "Bed", Detail = "1 King Bed Frame", Quantity = 1 },
                    new InventoryItem { Name = "Chairs", Detail = "3 Chairs (2 Balcony + 1 Desk)", Quantity = 3 },
                    new InventoryItem { Name = "Desk", Detail = "1 Folding Work Desk", Quantity = 1 },
                    new InventoryItem { Name = "Wardrobe", Detail = "1 Built-in Almirah", Quantity = 1 }
                },
                SharedAmenities = new List<SharedAmenity>
                {
                    new SharedAmenity { Name = "Double Door Refrigerator" },
                    new SharedAmenity { Name = "Automatic Washing Machine" },
                    new SharedAmenity { Name = "RO Water Filter" }
                }
            },
            new Listing
            {
                Title = "Shared Double Bed in High-rise 3 BHK (Tech Corridor)",
                TotalBhk = "3 BHK",
                Offering = "1 Shared Bed in Double Room",
                Location = "Hitec City, Hyderabad",
                Rent = 8500,
                Deposit = 17000,
                AvailableDate = "Next Week",
                Bills = "Split 50/50 evenly",
                Condition = "Functional / Fair",
                TenantName = "Dev M.",
                TenantInitial = "DM",
                TenantEmail = "dev@example.com",
                TenantRole = "Primary Tenant",
                Images = new List<ListingImage>
                {
                    new ListingImage { Url = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?auto=format&fit=crop&w=800&q=80", IsCover = true, Source = "stock" }
                },
                Inventory = new List<InventoryItem>
                {
                    new InventoryItem { Name = "Bed", Detail = "1 Single Bed", Quantity = 1 },
                    new InventoryItem { Name = "Chairs", Detail = "1 Study Chair", Quantity = 1 },
                    new InventoryItem { Name = "Desk", Detail = "1 Compact Study Table", Quantity = 1 }
                },
                SharedAmenities = new List<SharedAmenity>
                {
                    new SharedAmenity { Name = "Fridge" },
                    new SharedAmenity { Name = "Washing Machine" },
                    new SharedAmenity { Name = "Wi-Fi Router" }
                }
            }
        };

        context.Listings.AddRange(listings);
        context.SaveChanges();
    }
}
