namespace SubletNest.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    
    // Category e.g. "Bed", "Chairs", "Desk", "Wardrobe", "AC", "Appliance"
    public string Name { get; set; } = string.Empty;
    
    // Exact detail e.g. "1 Queen Bed with Ortho Mattress", "2 Ergonomic Study Chairs"
    public string Detail { get; set; } = string.Empty;

    // Piece count
    public int Quantity { get; set; } = 1;

    // Condition e.g. "Gently Used", "Brand New", "Well Maintained"
    public string? ItemCondition { get; set; }
}
