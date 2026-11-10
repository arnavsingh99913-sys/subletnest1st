using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SubletNest.Data;

var contentRoot = Directory.GetCurrentDirectory();
var siblingFrontend = Path.GetFullPath(Path.Combine(contentRoot, "..", "frontend"));
var directFrontend = Path.GetFullPath(Path.Combine(contentRoot, "frontend"));

string webRoot = Directory.Exists(siblingFrontend) 
    ? siblingFrontend 
    : (Directory.Exists(directFrontend) ? directFrontend : Path.Combine(contentRoot, "wwwroot"));

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = webRoot
});

// Add Database Context (SQLite)
builder.Services.AddDbContext<SubletDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=subletnest.db"));

// Add CORS Policy for flexible frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Form options for Camera snapshots & high-res image uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 25 * 1024 * 1024; // 25 MB
});

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

// Ensure uploads directory exists in WebRootPath
var resolvedWebRoot = app.Environment.WebRootPath;
if (string.IsNullOrEmpty(resolvedWebRoot))
{
    resolvedWebRoot = webRoot;
}
var uploadsDir = Path.Combine(resolvedWebRoot, "uploads");
if (!Directory.Exists(uploadsDir))
{
    Directory.CreateDirectory(uploadsDir);
}

// Auto-migrate and seed database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SubletDbContext>();
    var initLogger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");
    DbInitializer.Initialize(db, app.Configuration, initLogger);
}

app.UseCors("AllowAll");

// Serve static files from frontend (HTML, JS, CSS, and /uploads/*)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

// Health check endpoint
app.MapGet("/api/health", () => Results.Ok(new
{
    status = "healthy",
    service = "SubletNest API (.NET 8)",
    webRoot = app.Environment.WebRootPath,
    timestamp = DateTime.UtcNow
}));

// Fallback to index.html for SPA client-side routing
app.MapFallbackToFile("index.html");

app.Run();
