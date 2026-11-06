namespace SubletNest.DTOs;

public class CameraUploadRequest
{
    // Base64 encoded image string (e.g., data:image/jpeg;base64,/9j/4AAQSkZJRg...)
    public string ImageBase64 { get; set; } = string.Empty;

    // Optional filename
    public string? FileName { get; set; }

    // Source e.g. "live-camera", "gallery-upload"
    public string Source { get; set; } = "live-camera";
}

public class UploadResponse
{
    public bool Success { get; set; } = true;
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
