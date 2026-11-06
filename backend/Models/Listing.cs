namespace SubletNest.Models;

public class Listing
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TotalBhk { get; set; } = "2 BHK"; // 2 BHK, 3 BHK, 4 BHK, 1.5 BHK
    public string Offering { get; set; } = "1 Private Bedroom"; // 1 Master Bed with Attached Bath, 1 Private Bedroom, 1 Shared Bed, Entire 1 BHK Portion
    public string Location { get; set; } = string.Empty;
    public decimal Rent { get; set; } // Monthly rent share for the extra room
    public decimal Deposit { get; set; } // Security deposit share
    public string AvailableDate { get; set; } = "Immediate Move-in";
    public string Bills { get; set; } = "Split 50/50 evenly"; // Split 50/50 evenly, Included in Rent, Metered separately
    public string Condition { get; set; } = "Gently Used - Great"; // Gently Used - Great, Brand New, Well Maintained, Functional / Fair
    
    // Host / Primary Tenant info (the person renting out spare room)
    public string TenantName { get; set; } = string.Empty;
    public string TenantEmail { get; set; } = string.Empty;
    public string? TenantPhone { get; set; }
    public string TenantInitial { get; set; } = "TN";
    public string TenantRole { get; set; } = "Primary Tenant"; // Primary Leaseholder

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public List<ListingImage> Images { get; set; } = new();
    public List<InventoryItem> Inventory { get; set; } = new();
    public List<SharedAmenity> SharedAmenities { get; set; } = new();
    public List<Inquiry> Inquiries { get; set; } = new();
}
