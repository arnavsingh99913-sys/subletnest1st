namespace SubletNest.Models;

public class Inquiry
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    
    public string SeekerName { get; set; } = string.Empty;
    public string SeekerEmail { get; set; } = string.Empty;
    public string SeekerPhone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string MoveInDate { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty; // e.g. Software Engineer, Student
    public string Status { get; set; } = "Pending"; // Pending, Accepted, Declined
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
