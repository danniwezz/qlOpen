namespace CustomerModule.Public;

public class AssignedServiceDto
{
	public long Id { get; set; }
    public long ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public long CustomerId { get; set; }
    public ICollection<DiscountDto> Discounts { get; set; } = new HashSet<DiscountDto>();
}
