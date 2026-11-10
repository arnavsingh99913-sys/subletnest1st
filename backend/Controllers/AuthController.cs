using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubletNest.Data;
using SubletNest.DTOs;
using SubletNest.Models;

namespace SubletNest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SubletDbContext _context;
    private readonly ILogger<AuthController> _logger;

    public AuthController(SubletDbContext context, ILogger<AuthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Sign Up / Register a new Leaser or User with name, contact, email, and password
    /// </summary>
    [HttpPost("signup")]
    public async Task<ActionResult<UserResponseDto>> SignUp([FromBody] SignUpDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new { message = "Registration data is required." });
        }

        var name = dto.Name?.Trim() ?? string.Empty;
        var contact = dto.Contact?.Trim() ?? string.Empty;
        var email = dto.Email?.Trim() ?? string.Empty;
        var password = dto.Password ?? string.Empty;

        // Compulsory fields validation
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { message = "Full name is compulsory for registration." });
        }

        if (string.IsNullOrWhiteSpace(contact))
        {
            return BadRequest(new { message = "Contact number is compulsory for registration." });
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { message = "Email address is compulsory for registration." });
        }

        if (!email.Contains('@') || !email.Contains('.'))
        {
            return BadRequest(new { message = "Please provide a valid email address." });
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
        {
            return BadRequest(new { message = "Password is compulsory and must be at least 4 characters." });
        }

        var lowerEmail = email.ToLowerInvariant();

        // Check duplicate email
        var existingEmail = await _context.Users
            .AnyAsync(u => u.Email.ToLower() == lowerEmail);
        if (existingEmail)
        {
            return BadRequest(new { message = "An account with this email address already exists. Please sign in instead." });
        }

        // Check duplicate contact
        var existingContact = await _context.Users
            .AnyAsync(u => u.Contact == contact);
        if (existingContact)
        {
            return BadRequest(new { message = "An account with this contact number already exists. Please sign in instead." });
        }

        // Generate Initials from Name
        var initials = GenerateInitials(name);

        // Sign-up can only create Buyers or Sellers. The administrator is created from configuration at startup.
        var newUser = new User
        {
            Name = name,
            Contact = contact,
            Email = email,
            PasswordHash = HashPassword(password),
            Role = NormalizeRole(dto.Role),
            Initials = initials,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("New user registered successfully: {Email} ({Name})", newUser.Email, newUser.Name);

        return CreatedAtAction(nameof(GetProfile), new { id = newUser.Id }, new UserResponseDto
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Contact = newUser.Contact,
            Email = newUser.Email,
            Role = newUser.Role,
            Initials = newUser.Initials,
            CreatedAt = newUser.CreatedAt
        });
    }

    /// <summary>
    /// Sign In / Login using Name, Email, OR Contact (any one compulsory) plus Password
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<UserResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new { message = "Login credentials are required." });
        }

        // Find identifier: could be passed in Identifier, Email, Contact, or Name
        var identifier = (dto.Identifier ?? dto.Email ?? dto.Contact ?? dto.Name ?? string.Empty).Trim();
        var password = dto.Password ?? string.Empty;

        // Any one of Name, Email, or Contact is compulsory
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return BadRequest(new { message = "Login identifier is compulsory. Please enter your Name, Email, or Contact number." });
        }

        // Password is compulsory
        if (string.IsNullOrWhiteSpace(password))
        {
            return BadRequest(new { message = "Password is compulsory. Please enter your password." });
        }

        var lowerIdentifier = identifier.ToLowerInvariant();

        // Search database matching any one: Email, Contact, or Name
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Email.ToLower() == lowerIdentifier ||
                u.Contact == identifier ||
                u.Name.ToLower() == lowerIdentifier);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials. No account found matching this Name, Email, or Contact number." });
        }

        // Verify password
        if (!VerifyPassword(password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid password. Please check your credentials and try again." });
        }

        _logger.LogInformation("User logged in successfully: {Email} via identifier '{Identifier}'", user.Email, identifier);

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Contact = user.Contact,
            Email = user.Email,
            Role = user.Role,
            Initials = user.Initials,
            CreatedAt = user.CreatedAt
        });
    }

    /// <summary>
    /// Get user profile by Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponseDto>> GetProfile(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound(new { message = "User not found." });

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Contact = user.Contact,
            Email = user.Email,
            Role = user.Role,
            Initials = user.Initials,
            CreatedAt = user.CreatedAt
        });
    }

    /// <summary>
    /// List all registered users with optional search by Name or Contact number
    /// </summary>
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers([FromQuery] string? search = null)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLowerInvariant();
            query = query.Where(u => 
                u.Name.ToLower().Contains(s) || 
                u.Contact.Contains(s) || 
                u.Email.ToLower().Contains(s));
        }

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Contact = u.Contact,
                Email = u.Email,
                Role = u.Role,
                Initials = u.Initials,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    public const string RoleBuyer = "Buyer";
    public const string RoleSeller = "Seller";

    /// <summary>
    /// Only two self-service roles exist: Buyer (looking for a room) and Seller (listing a room).
    /// Anything else, including a client asking for "Administrator", becomes Buyer.
    /// </summary>
    private static string NormalizeRole(string? role)
    {
        var r = role?.Trim() ?? string.Empty;
        if (r.Equals(RoleSeller, StringComparison.OrdinalIgnoreCase) ||
            r.Equals("Primary Leaseholder", StringComparison.OrdinalIgnoreCase))
        {
            return RoleSeller;
        }
        return RoleBuyer;
    }

    private static string GenerateInitials(string name)
    {
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            return $"{char.ToUpperInvariant(parts[0][0])}{char.ToUpperInvariant(parts[1][0])}";
        }
        if (name.Length >= 2)
        {
            return name[..2].ToUpperInvariant();
        }
        if (name.Length == 1)
        {
            return name.ToUpperInvariant();
        }
        return "US";
    }

    // Secure PBKDF2 Password Hashing
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split(':');
        if (parts.Length != 2) return false;

        try
        {
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hash = Convert.FromBase64String(parts[1]);
            byte[] testHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);
            return CryptographicOperations.FixedTimeEquals(hash, testHash);
        }
        catch
        {
            return false;
        }
    }
}

