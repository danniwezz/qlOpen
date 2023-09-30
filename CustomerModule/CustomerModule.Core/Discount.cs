namespace CustomerModule.Core;
public class Discount
{
    public long Id { get; set; }
    public long AssignedServiceId { get; set; }
    public decimal Percentage { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
