namespace SubletNest.DTOs;

public class CreateInquiryRequest
{
    public int ListingId { get; set; }
    public string SeekerName { get; set; } = string.Empty;
    public string SeekerEmail { get; set; } = string.Empty;
    public string SeekerPhone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string MoveInDate { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
}

public class InquiryDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public string SeekerName { get; set; } = string.Empty;
    public string SeekerEmail { get; set; } = string.Empty;
    public string SeekerPhone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string MoveInDate { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}
