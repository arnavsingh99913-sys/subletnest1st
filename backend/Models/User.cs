namespace SubletNest.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Primary Leaseholder";
    public string Initials { get; set; } = "US";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

