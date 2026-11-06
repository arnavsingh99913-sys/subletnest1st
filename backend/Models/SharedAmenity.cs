namespace SubletNest.Models;

public class SharedAmenity
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    
    // E.g. "3-Seater Sofa", "Double Door Fridge", "Washing Machine", "RO Water Filter", "Microwave"
    public string Name { get; set; } = string.Empty;
}
