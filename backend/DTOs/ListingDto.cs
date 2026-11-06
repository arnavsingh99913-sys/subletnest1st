namespace SubletNest.DTOs;

public class InventoryItemDto
{
    public string Name { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public string? ItemCondition { get; set; }
}

public class ListingDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TotalBhk { get; set; } = string.Empty;
    public string Offering { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal Rent { get; set; }
    public decimal Deposit { get; set; }
    public string AvailableDate { get; set; } = string.Empty;
    public string Bills { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    
    public string TenantName { get; set; } = string.Empty;
    public string TenantInitial { get; set; } = string.Empty;
    public string TenantEmail { get; set; } = string.Empty;
    public string? TenantPhone { get; set; }
    public string TenantRole { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<string> Images { get; set; } = new();
    public List<InventoryItemDto> Inventory { get; set; } = new();
    public List<string> Shared { get; set; } = new();
}

public class CreateListingRequest
{
    public string Title { get; set; } = string.Empty;
    public string TotalBhk { get; set; } = "2 BHK";
    public string Offering { get; set; } = "1 Private Bedroom";
    public string Location { get; set; } = string.Empty;
    public decimal Rent { get; set; }
    public decimal Deposit { get; set; }
    public string AvailableDate { get; set; } = "Immediate Move-in";
    public string Bills { get; set; } = "Split 50/50 evenly";
    public string Condition { get; set; } = "Gently Used - Great";

    public string TenantName { get; set; } = string.Empty;
    public string TenantInitial { get; set; } = string.Empty;
    public string TenantEmail { get; set; } = string.Empty;
    public string? TenantPhone { get; set; }

    public List<string> Images { get; set; } = new();
    public List<InventoryItemDto> Inventory { get; set; } = new();
    public List<string> Shared { get; set; } = new();
}
