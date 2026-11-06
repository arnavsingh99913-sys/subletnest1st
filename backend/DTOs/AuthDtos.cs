namespace SubletNest.DTOs;

public class SignUpDto
{
    public string Name { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Role { get; set; }
}

public class LoginDto
{
    public string? Identifier { get; set; } // Can be name, email, or contact
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Contact { get; set; }
    public string Password { get; set; } = string.Empty;
}

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

