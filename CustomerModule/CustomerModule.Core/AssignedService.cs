namespace CustomerModule.Core;
public class AssignedService
{
    public long Id { get; set; }
    public long ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public long CustomerId { get; set; }
    public ICollection<Discount> Discounts { get; set; } = new HashSet<Discount>();
}
