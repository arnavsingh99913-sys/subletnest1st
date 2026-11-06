using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubletNest.Data;
using SubletNest.DTOs;
using SubletNest.Models;

namespace SubletNest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly SubletDbContext _context;
    private readonly ILogger<ListingsController> _logger;

    private static readonly Dictionary<string, string[]> CityAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        { "bengaluru", new[] { "bengaluru", "bangalore" } },
        { "bangalore", new[] { "bengaluru", "bangalore" } },
        { "mumbai", new[] { "mumbai", "bombay" } },
        { "bombay", new[] { "mumbai", "bombay" } },
        { "gurugram", new[] { "gurugram", "gurgaon" } },
        { "gurgaon", new[] { "gurugram", "gurgaon" } },
        { "kolkata", new[] { "kolkata", "calcutta" } },
        { "calcutta", new[] { "kolkata", "calcutta" } },
        { "chennai", new[] { "chennai", "madras" } },
        { "madras", new[] { "chennai", "madras" } },
        { "mysuru", new[] { "mysuru", "mysore" } },
        { "mysore", new[] { "mysuru", "mysore" } },
        { "mangaluru", new[] { "mangaluru", "mangalore" } },
        { "mangalore", new[] { "mangaluru", "mangalore" } },
        { "hubballi", new[] { "hubballi", "hubli" } },
        { "hubli", new[] { "hubballi", "hubli" } },
        { "pune", new[] { "pune", "poona" } },
        { "poona", new[] { "pune", "poona" } },
        { "delhi", new[] { "delhi", "new delhi", "ncr" } },
        { "new delhi", new[] { "delhi", "new delhi", "ncr" } },
        { "varanasi", new[] { "varanasi", "banaras", "kashi" } },
        { "prayagraj", new[] { "prayagraj", "allahabad" } },
        { "vadodara", new[] { "vadodara", "baroda" } },
        { "thiruvananthapuram", new[] { "thiruvananthapuram", "trivandrum" } },
        { "kochi", new[] { "kochi", "cochin" } },
        { "kozhikode", new[] { "kozhikode", "calicut" } },
        { "chhatrapati sambhajinagar", new[] { "chhatrapati sambhajinagar", "aurangabad" } },
        { "aurangabad", new[] { "chhatrapati sambhajinagar", "aurangabad" } },
    };

    private static bool IsStopWord(string token)
    {
        return token is "in" or "at" or "the" or "near" or "for" or "and" or "of" or "to" or "with" or "all" or "is";
    }

    public ListingsController(SubletDbContext context, ILogger<ListingsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all partnership sublet listings with optional filtering and search
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListingDto>>> GetListings(
        [FromQuery] string? search = null,
        [FromQuery] string? bhk = null,
        [FromQuery] string? roomType = null,
        [FromQuery] string? furnishing = null,
        [FromQuery] decimal? maxBudget = null,
        [FromQuery] string? location = null,
        [FromQuery] string? city = null,
        [FromQuery] string? tenantEmail = null)
    {
        var query = _context.Listings
            .Include(l => l.Images)
            .Include(l => l.Inventory)
            .Include(l => l.SharedAmenities)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tenantEmail))
        {
            var lowerEmail = tenantEmail.Trim().ToLowerInvariant();
            query = query.Where(l => l.TenantEmail.ToLower() == lowerEmail);
        }

        if (!string.IsNullOrWhiteSpace(bhk) && bhk != "all")
        {
            var bhkKey = bhk.Split(' ')[0]; // e.g. "2" from "2 BHK"
            query = query.Where(l => l.TotalBhk.Contains(bhkKey));
        }

        if (!string.IsNullOrWhiteSpace(roomType) && roomType != "all")
        {
            var lowerRoom = roomType.ToLowerInvariant();
            query = query.Where(l => l.Offering.ToLower().Contains(lowerRoom));
        }

        if (maxBudget.HasValue && maxBudget.Value > 0)
        {
            query = query.Where(l => l.Rent <= maxBudget.Value);
        }

        var results = await query.OrderByDescending(l => l.CreatedAt).ToListAsync();

        // City filtering with alias normalization (e.g. Bangalore vs Bengaluru)
        if (!string.IsNullOrWhiteSpace(city) && city != "all")
        {
            var rawCity = city.Trim().ToLowerInvariant();
            var cityParts = rawCity.Split(new[] { '(', ')', '/', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => !string.IsNullOrEmpty(c))
                .ToList();

            var searchAliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var cp in cityParts)
            {
                searchAliases.Add(cp);
                if (CityAliases.TryGetValue(cp, out var aliases))
                {
                    foreach (var a in aliases) searchAliases.Add(a);
                }
            }

            results = results.Where(l => {
                var loc = l.Location.ToLowerInvariant();
                return searchAliases.Any(alias => loc.Contains(alias));
            }).ToList();
        }

        // Multi-keyword address shortlisting: street name, city name, state name, neighborhood
        var searchTarget = (!string.IsNullOrWhiteSpace(search) ? search : location)?.Trim();
        if (!string.IsNullOrWhiteSpace(searchTarget) && searchTarget != "all")
        {
            var lowerSearch = searchTarget.ToLowerInvariant();
            var searchTokens = lowerSearch
                .Split(new[] { ' ', ',', '-', '/', '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(t => t.Length > 1 && !IsStopWord(t))
                .ToList();

            if (searchTokens.Count > 0)
            {
                results = results.Where(l =>
                {
                    var loc = l.Location.ToLowerInvariant();
                    var title = l.Title.ToLowerInvariant();
                    var bhkStr = l.TotalBhk.ToLowerInvariant();
                    var offeringStr = l.Offering.ToLowerInvariant();

                    // Direct whole phrase match
                    if (loc.Contains(lowerSearch) || title.Contains(lowerSearch)) return true;

                    // Keyword matching against address parts (street name, locality, city, state)
                    var locMatches = searchTokens.Any(t => loc.Contains(t));
                    var titleMatches = searchTokens.Any(t => title.Contains(t));
                    var invMatches = l.Inventory.Any(i => searchTokens.Any(t => i.Detail.ToLowerInvariant().Contains(t) || i.Name.ToLowerInvariant().Contains(t)));
                    var sharedMatches = l.SharedAmenities.Any(s => searchTokens.Any(t => s.Name.ToLowerInvariant().Contains(t)));

                    return locMatches || titleMatches || invMatches || sharedMatches;
                })
                .OrderByDescending(l =>
                {
                    var loc = l.Location.ToLowerInvariant();
                    var title = l.Title.ToLowerInvariant();
                    int relevance = 0;
                    if (loc.Contains(lowerSearch)) relevance += 100;
                    if (title.Contains(lowerSearch)) relevance += 50;
                    foreach (var t in searchTokens)
                    {
                        if (loc.Contains(t)) relevance += 25;
                        if (title.Contains(t)) relevance += 10;
                    }
                    return relevance;
                })
                .ToList();
            }
        }

        if (!string.IsNullOrWhiteSpace(furnishing) && furnishing != "all")
        {
            if (furnishing == "Has Study Setup")
            {
                results = results.Where(l => l.Inventory.Any(i => 
                    i.Name.Contains("Desk", StringComparison.OrdinalIgnoreCase) || 
                    i.Name.Contains("Chairs", StringComparison.OrdinalIgnoreCase) ||
                    i.Detail.Contains("Desk", StringComparison.OrdinalIgnoreCase) ||
                    i.Detail.Contains("Chair", StringComparison.OrdinalIgnoreCase)
                )).ToList();
            }
            else if (furnishing == "Has AC")
            {
                results = results.Where(l => l.Inventory.Any(i => 
                    i.Name.Contains("AC", StringComparison.OrdinalIgnoreCase) ||
                    i.Detail.Contains("AC", StringComparison.OrdinalIgnoreCase)
                )).ToList();
            }
        }

        var dtos = results.Select(MapToDto).ToList();
        return Ok(dtos);
    }

    /// <summary>
    /// Get details of a single sublet room
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ListingDto>> GetListing(int id)
    {
        var listing = await _context.Listings
            .Include(l => l.Images)
            .Include(l => l.Inventory)
            .Include(l => l.SharedAmenities)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (listing == null)
        {
            return NotFound(new { message = $"Listing with ID {id} not found." });
        }

        return Ok(MapToDto(listing));
    }

    /// <summary>
    /// Publish a spare room partnership sublet with leftover furniture inventory
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ListingDto>> CreateListing([FromBody] CreateListingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { message = "Listing title is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Location))
        {
            return BadRequest(new { message = "Location (city & neighborhood) is required." });
        }

        if (request.Rent <= 0)
        {
            return BadRequest(new { message = "Rent must be greater than 0." });
        }

        var initials = !string.IsNullOrWhiteSpace(request.TenantInitial) 
            ? request.TenantInitial.ToUpperInvariant() 
            : GetInitials(request.TenantName);

        var listing = new Listing
        {
            Title = request.Title.Trim(),
            TotalBhk = request.TotalBhk,
            Offering = request.Offering,
            Location = request.Location.Trim(),
            Rent = request.Rent,
            Deposit = request.Deposit,
            AvailableDate = string.IsNullOrWhiteSpace(request.AvailableDate) ? "Immediate Move-in" : request.AvailableDate.Trim(),
            Bills = string.IsNullOrWhiteSpace(request.Bills) ? "Split 50/50 evenly" : request.Bills,
            Condition = string.IsNullOrWhiteSpace(request.Condition) ? "Gently Used - Great" : request.Condition,
            TenantName = string.IsNullOrWhiteSpace(request.TenantName) ? "Primary Tenant" : request.TenantName.Trim(),
            TenantEmail = string.IsNullOrWhiteSpace(request.TenantEmail) ? "tenant@example.com" : request.TenantEmail.Trim(),
            TenantPhone = request.TenantPhone?.Trim(),
            TenantInitial = initials,
            TenantRole = "Primary Tenant",
            CreatedAt = DateTime.UtcNow
        };

        // Attach photos
        if (request.Images != null && request.Images.Count > 0)
        {
            for (int i = 0; i < request.Images.Count; i++)
            {
                var imgUrl = request.Images[i];
                if (!string.IsNullOrWhiteSpace(imgUrl))
                {
                    listing.Images.Add(new ListingImage
                    {
                        Url = imgUrl.Trim(),
                        IsCover = (i == 0),
                        Source = imgUrl.Contains("camera") ? "camera" : "gallery",
                        UploadedAt = DateTime.UtcNow
                    });
                }
            }
        }
        else
        {
            // Default placeholder room photo
            listing.Images.Add(new ListingImage
            {
                Url = "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?auto=format&fit=crop&w=800&q=80",
                IsCover = true,
                Source = "stock",
                UploadedAt = DateTime.UtcNow
            });
        }

        // Attach inventory
        if (request.Inventory != null)
        {
            foreach (var inv in request.Inventory)
            {
                if (!string.IsNullOrWhiteSpace(inv.Detail))
                {
                    listing.Inventory.Add(new InventoryItem
                    {
                        Name = string.IsNullOrWhiteSpace(inv.Name) ? "Item" : inv.Name,
                        Detail = inv.Detail.Trim(),
                        Quantity = inv.Quantity > 0 ? inv.Quantity : 1,
                        ItemCondition = inv.ItemCondition ?? listing.Condition
                    });
                }
            }
        }

        // Attach shared amenities
        if (request.Shared != null)
        {
            foreach (var s in request.Shared)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    listing.SharedAmenities.Add(new SharedAmenity { Name = s.Trim() });
                }
            }
        }

        _context.Listings.Add(listing);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Listing published: ID {Id}, Title: {Title}", listing.Id, listing.Title);

        var dto = MapToDto(listing);
        return CreatedAtAction(nameof(GetListing), new { id = listing.Id }, dto);
    }

    /// <summary>
    /// Update an existing sublet listing
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ListingDto>> UpdateListing(int id, [FromBody] CreateListingRequest request)
    {
        var listing = await _context.Listings
            .Include(l => l.Images)
            .Include(l => l.Inventory)
            .Include(l => l.SharedAmenities)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (listing == null)
        {
            return NotFound(new { message = $"Listing with ID {id} not found." });
        }

        listing.Title = request.Title.Trim();
        listing.TotalBhk = request.TotalBhk;
        listing.Offering = request.Offering;
        listing.Location = request.Location.Trim();
        listing.Rent = request.Rent;
        listing.Deposit = request.Deposit;
        listing.AvailableDate = request.AvailableDate;
        listing.Bills = request.Bills;
        listing.Condition = request.Condition;

        // Replace images
        listing.Images.Clear();
        if (request.Images != null)
        {
            for (int i = 0; i < request.Images.Count; i++)
            {
                listing.Images.Add(new ListingImage
                {
                    Url = request.Images[i],
                    IsCover = (i == 0),
                    Source = request.Images[i].Contains("camera") ? "camera" : "upload"
                });
            }
        }

        // Replace inventory
        listing.Inventory.Clear();
        if (request.Inventory != null)
        {
            foreach (var inv in request.Inventory)
            {
                listing.Inventory.Add(new InventoryItem
                {
                    Name = inv.Name,
                    Detail = inv.Detail,
                    Quantity = inv.Quantity,
                    ItemCondition = inv.ItemCondition
                });
            }
        }

        // Replace shared
        listing.SharedAmenities.Clear();
        if (request.Shared != null)
        {
            foreach (var s in request.Shared)
            {
                listing.SharedAmenities.Add(new SharedAmenity { Name = s });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(MapToDto(listing));
    }

    /// <summary>
    /// Delete a listing from profile and marketplace, preserving record in Admin Audit Database
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteListing(int id)
    {
        var listing = await _context.Listings.FindAsync(id);
        if (listing == null)
        {
            return NotFound(new { message = $"Listing with ID {id} not found." });
        }

        // Record in Admin Database before removal
        var audit = new ListingAudit
        {
            ListingId = listing.Id,
            Title = listing.Title,
            Location = listing.Location,
            Rent = listing.Rent,
            TenantName = listing.TenantName,
            TenantEmail = listing.TenantEmail,
            TenantPhone = listing.TenantPhone,
            ListedAt = listing.CreatedAt,
            RemovedAt = DateTime.UtcNow,
            Action = "Removed from website"
        };
        _context.ListingAudits.Add(audit);

        _context.Listings.Remove(listing);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted listing ID: {Id} from website; logged to Admin Database", id);
        return Ok(new { success = true, message = "Listing removed from website and preserved in Admin audit database." });
    }

    /// <summary>
    /// Admin database endpoint: Retrieve all listed rooms history (active & removed)
    /// </summary>
    [HttpGet("admin/audit")]
    public async Task<IActionResult> GetAdminAudit()
    {
        var activeListings = await _context.Listings
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new
            {
                id = l.Id,
                title = l.Title,
                location = l.Location,
                rent = l.Rent,
                tenantName = l.TenantName,
                tenantEmail = l.TenantEmail,
                tenantPhone = l.TenantPhone,
                listedAt = l.CreatedAt,
                removedAt = (DateTime?)null,
                status = "Active on Website"
            })
            .ToListAsync();

        var removedListings = await _context.ListingAudits
            .OrderByDescending(a => a.RemovedAt)
            .Select(a => new
            {
                id = a.ListingId,
                title = a.Title,
                location = a.Location,
                rent = a.Rent,
                tenantName = a.TenantName,
                tenantEmail = a.TenantEmail,
                tenantPhone = a.TenantPhone,
                listedAt = a.ListedAt,
                removedAt = (DateTime?)a.RemovedAt,
                status = "Removed from Website"
            })
            .ToListAsync();

        return Ok(new
        {
            active = activeListings,
            removed = removedListings
        });
    }

    /// <summary>
    /// Summary stats for the marketplace
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var totalListings = await _context.Listings.CountAsync();
        
        // SQLite does not support decimal AverageAsync directly in SQL; compute over loaded rents or cast
        var rents = await _context.Listings.Select(l => (double)l.Rent).ToListAsync();
        var avgRent = rents.Count > 0 ? rents.Average() : 0;
        
        var totalInquiries = await _context.Inquiries.CountAsync();
        var cities = await _context.Listings
            .Select(l => l.Location)
            .Distinct()
            .Take(10)
            .ToListAsync();

        return Ok(new
        {
            totalListings,
            averageRent = Math.Round(avgRent, 0),
            totalInquiries,
            popularLocations = cities
        });
    }

    private static ListingDto MapToDto(Listing l)
    {
        return new ListingDto
        {
            Id = l.Id,
            Title = l.Title,
            TotalBhk = l.TotalBhk,
            Offering = l.Offering,
            Location = l.Location,
            Rent = l.Rent,
            Deposit = l.Deposit,
            AvailableDate = l.AvailableDate,
            Bills = l.Bills,
            Condition = l.Condition,
            TenantName = l.TenantName,
            TenantInitial = l.TenantInitial,
            TenantEmail = l.TenantEmail,
            TenantPhone = l.TenantPhone,
            TenantRole = l.TenantRole,
            CreatedAt = l.CreatedAt,
            Images = l.Images.OrderByDescending(img => img.IsCover).Select(img => img.Url).ToList(),
            Inventory = l.Inventory.Select(inv => new InventoryItemDto
            {
                Name = inv.Name,
                Detail = inv.Detail,
                Quantity = inv.Quantity,
                ItemCondition = inv.ItemCondition
            }).ToList(),
            Shared = l.SharedAmenities.Select(s => s.Name).ToList()
        };
    }

    private static string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "TN";
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1) return parts[0][0].ToString().ToUpperInvariant();
        return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
    }
}
