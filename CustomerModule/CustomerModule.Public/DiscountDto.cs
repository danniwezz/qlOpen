namespace CustomerModule.Public;

public class DiscountDto
{
    public long Id { get; set; }
    public long AssignedServiceId { get; set; }
    public decimal Percentage { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
