namespace SubletNest.Models;

public class ListingImage
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsCover { get; set; }
    public string? Caption { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Source indicator: e.g. "camera", "gallery", "stock"
    public string Source { get; set; } = "upload";
}
