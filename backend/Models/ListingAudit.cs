namespace SubletNest.Models;

public class ListingAudit
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal Rent { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string TenantEmail { get; set; } = string.Empty;
    public string? TenantPhone { get; set; }
    public DateTime ListedAt { get; set; } = DateTime.UtcNow;
    public DateTime RemovedAt { get; set; } = DateTime.UtcNow;
    public string Action { get; set; } = "Removed from site";
}

