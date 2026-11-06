using Microsoft.AspNetCore.Mvc;
using SubletNest.DTOs;
using System.Text.RegularExpressions;

namespace SubletNest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<UploadController> _logger;

    public UploadController(IWebHostEnvironment env, ILogger<UploadController> logger)
    {
        _env = env;
        _logger = logger;
    }

    /// <summary>
    /// Handles camera snapshots and file uploads via multipart/form-data
    /// </summary>
    [HttpPost]
    [RequestSizeLimit(15 * 1024 * 1024)] // 15 MB
    public async Task<ActionResult<UploadResponse>> UploadFile([FromForm] IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new UploadResponse { Success = false, Message = "No file was uploaded." });
        }

        // Validate image extension
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        if (!allowedExtensions.Contains(ext))
        {
            return BadRequest(new UploadResponse { Success = false, Message = "Only image files (.jpg, .jpeg, .png, .webp) are supported." });
        }

        var uploadsDir = GetUploadsDirectory();
        var uniqueFileName = $"room_{Guid.NewGuid():N}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{ext}";
        var filePath = Path.Combine(uploadsDir, uniqueFileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        _logger.LogInformation("Image uploaded successfully: {FileName}, size: {Size} bytes", uniqueFileName, file.Length);

        return Ok(new UploadResponse
        {
            Success = true,
            Url = $"/uploads/{uniqueFileName}",
            FileName = uniqueFileName,
            Size = file.Length,
            Source = "file-upload",
            Message = "Photo uploaded successfully."
        });
    }

    /// <summary>
    /// Dedicated endpoint to accept camera snapshots taken directly from the frontend camera button / canvas
    /// </summary>
    [HttpPost("camera")]
    [RequestSizeLimit(15 * 1024 * 1024)]
    public async Task<ActionResult<UploadResponse>> UploadCameraSnapshot([FromBody] CameraUploadRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ImageBase64))
        {
            return BadRequest(new UploadResponse { Success = false, Message = "Empty camera snapshot data." });
        }

        try
        {
            // Remove data:image/...;base64, header if present
            var base64Data = request.ImageBase64;
            var extension = ".jpg";

            var match = Regex.Match(base64Data, @"^data:image/(?<type>[a-zA-Z0-9\+\.]+);base64,(?<data>.+)$");
            if (match.Success)
            {
                var type = match.Groups["type"].Value.ToLowerInvariant();
                extension = type switch
                {
                    "png" => ".png",
                    "webp" => ".webp",
                    "gif" => ".gif",
                    _ => ".jpg"
                };
                base64Data = match.Groups["data"].Value;
            }

            var imageBytes = Convert.FromBase64String(base64Data);
            if (imageBytes.Length > 15 * 1024 * 1024)
            {
                return BadRequest(new UploadResponse { Success = false, Message = "Snapshot exceeds 15MB size limit." });
            }

            var uploadsDir = GetUploadsDirectory();
            var uniqueFileName = $"camera_{Guid.NewGuid():N}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{extension}";
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            _logger.LogInformation("Camera snapshot captured and saved: {FileName}, size: {Size} bytes", uniqueFileName, imageBytes.Length);

            return Ok(new UploadResponse
            {
                Success = true,
                Url = $"/uploads/{uniqueFileName}",
                FileName = uniqueFileName,
                Size = imageBytes.Length,
                Source = request.Source,
                Message = "Camera photo captured and saved to server."
            });
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Failed to decode base64 camera image");
            return BadRequest(new UploadResponse { Success = false, Message = "Invalid base64 image data." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving camera snapshot");
            return StatusCode(500, new UploadResponse { Success = false, Message = "Failed to process camera snapshot." });
        }
    }

    private string GetUploadsDirectory()
    {
        var webRoot = _env.WebRootPath;
        if (string.IsNullOrEmpty(webRoot))
        {
            var siblingFrontend = Path.GetFullPath(Path.Combine(_env.ContentRootPath, "..", "frontend"));
            webRoot = Directory.Exists(siblingFrontend) ? siblingFrontend : Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var uploadsDir = Path.Combine(webRoot, "uploads");
        if (!Directory.Exists(uploadsDir))
        {
            Directory.CreateDirectory(uploadsDir);
        }

        return uploadsDir;
    }
}
