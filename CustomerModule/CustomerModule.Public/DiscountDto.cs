namespace CustomerModule.Public;

public class DiscountDto
{
    public decimal Percentage { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
