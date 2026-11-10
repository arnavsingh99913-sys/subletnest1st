using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubletNest.Data;
using SubletNest.DTOs;
using SubletNest.Models;

namespace SubletNest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InquiriesController : ControllerBase
{
    private readonly SubletDbContext _context;
    private readonly ILogger<InquiriesController> _logger;

    public InquiriesController(SubletDbContext context, ILogger<InquiriesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Submit an inquiry to the primary tenant for room partnership
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<InquiryDto>> CreateInquiry([FromBody] CreateInquiryRequest request)
    {
        if (request.ListingId <= 0)
        {
            return BadRequest(new { message = "Valid Listing ID is required." });
        }

        var listing = await _context.Listings.FindAsync(request.ListingId);
        if (listing == null)
        {
            return NotFound(new { message = $"Listing with ID {request.ListingId} not found." });
        }

        if (string.IsNullOrWhiteSpace(request.SeekerName) || string.IsNullOrWhiteSpace(request.SeekerEmail))
        {
            return BadRequest(new { message = "Seeker name and email are required." });
        }

        var inquiry = new Inquiry
        {
            ListingId = request.ListingId,
            SeekerName = request.SeekerName.Trim(),
            SeekerEmail = request.SeekerEmail.Trim(),
            SeekerPhone = request.SeekerPhone?.Trim() ?? string.Empty,
            Message = string.IsNullOrWhiteSpace(request.Message) 
                ? "Hi! I am interested in partnering up and renting the spare room." 
                : request.Message.Trim(),
            MoveInDate = string.IsNullOrWhiteSpace(request.MoveInDate) ? "Immediate" : request.MoveInDate.Trim(),
            Occupation = string.IsNullOrWhiteSpace(request.Occupation) ? "Working Professional" : request.Occupation.Trim(),
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.Inquiries.Add(inquiry);
        await _context.SaveChangesAsync();

        _logger.LogInformation("New inquiry submitted for listing {ListingId} by {SeekerName}", request.ListingId, inquiry.SeekerName);

        var dto = new InquiryDto
        {
            Id = inquiry.Id,
            ListingId = inquiry.ListingId,
            ListingTitle = listing.Title,
            SeekerName = inquiry.SeekerName,
            SeekerEmail = inquiry.SeekerEmail,
            SeekerPhone = inquiry.SeekerPhone,
            Message = inquiry.Message,
            MoveInDate = inquiry.MoveInDate,
            Occupation = inquiry.Occupation,
            Status = inquiry.Status,
            CreatedAt = inquiry.CreatedAt
        };

        return Ok(dto);
    }

    /// <summary>
    /// Get inquiries for a specific listing
    /// </summary>
    [HttpGet("listing/{listingId:int}")]
    public async Task<ActionResult<IEnumerable<InquiryDto>>> GetInquiriesForListing(int listingId)
    {
        var listing = await _context.Listings.FindAsync(listingId);
        if (listing == null)
        {
            return NotFound(new { message = $"Listing {listingId} not found." });
        }

        var inquiries = await _context.Inquiries
            .Where(i => i.ListingId == listingId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new InquiryDto
            {
                Id = i.Id,
                ListingId = i.ListingId,
                ListingTitle = listing.Title,
                SeekerName = i.SeekerName,
                SeekerEmail = i.SeekerEmail,
                SeekerPhone = i.SeekerPhone,
                Message = i.Message,
                MoveInDate = i.MoveInDate,
                Occupation = i.Occupation,
                Status = i.Status,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync();

        return Ok(inquiries);
    }

    /// <summary>
    /// Get all inquiries submitted to a tenant's listings
    /// </summary>
    [HttpGet("my-inquiries")]
    public async Task<ActionResult<IEnumerable<InquiryDto>>> GetMyInquiries([FromQuery] string tenantEmail)
    {
        if (string.IsNullOrWhiteSpace(tenantEmail))
        {
            return BadRequest(new { message = "tenantEmail query parameter is required." });
        }

        var lowerEmail = tenantEmail.Trim().ToLowerInvariant();

        var inquiries = await (
            from inq in _context.Inquiries
            join list in _context.Listings on inq.ListingId equals list.Id
            where list.TenantEmail.ToLower() == lowerEmail
            orderby inq.CreatedAt descending
            select new InquiryDto
            {
                Id = inq.Id,
                ListingId = inq.ListingId,
                ListingTitle = list.Title,
                SeekerName = inq.SeekerName,
                SeekerEmail = inq.SeekerEmail,
                SeekerPhone = inq.SeekerPhone,
                Message = inq.Message,
                MoveInDate = inq.MoveInDate,
                Occupation = inq.Occupation,
                Status = inq.Status,
                CreatedAt = inq.CreatedAt
            }
        ).ToListAsync();

        return Ok(inquiries);
    }

    /// <summary>
    /// Get all inquiries a buyer has sent, with their current status
    /// </summary>
    [HttpGet("sent")]
    public async Task<ActionResult<IEnumerable<InquiryDto>>> GetSentInquiries([FromQuery] string seekerEmail)
    {
        if (string.IsNullOrWhiteSpace(seekerEmail))
        {
            return BadRequest(new { message = "seekerEmail query parameter is required." });
        }

        var lowerEmail = seekerEmail.Trim().ToLowerInvariant();

        var inquiries = await (
            from inq in _context.Inquiries
            join list in _context.Listings on inq.ListingId equals list.Id
            where inq.SeekerEmail.ToLower() == lowerEmail
            orderby inq.CreatedAt descending
            select new InquiryDto
            {
                Id = inq.Id,
                ListingId = inq.ListingId,
                ListingTitle = list.Title,
                SeekerName = inq.SeekerName,
                SeekerEmail = inq.SeekerEmail,
                SeekerPhone = inq.SeekerPhone,
                Message = inq.Message,
                MoveInDate = inq.MoveInDate,
                Occupation = inq.Occupation,
                Status = inq.Status,
                CreatedAt = inq.CreatedAt
            }
        ).ToListAsync();

        return Ok(inquiries);
    }

    /// <summary>
    /// Accept or decline an inquiry
    /// </summary>
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateInquiryStatus(int id, [FromBody] UpdateInquiryStatusRequest req)
    {
        var inquiry = await _context.Inquiries.FindAsync(id);
        if (inquiry == null)
        {
            return NotFound(new { message = $"Inquiry {id} not found." });
        }

        inquiry.Status = req.Status;
        await _context.SaveChangesAsync();

        return Ok(new { success = true, id = inquiry.Id, status = inquiry.Status });
    }
}

public class UpdateInquiryStatusRequest
{
    public string Status { get; set; } = "Accepted"; // Accepted or Declined
}
