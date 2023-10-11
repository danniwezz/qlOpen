namespace CustomerModule.Public;

public class AssignedServiceDto
{
    public long ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
    public int ValidFromWeekDayNumber { get; set; }
	public int ValidToWeekDayNumber { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ICollection<DiscountDto> Discounts { get; set; } = new HashSet<DiscountDto>();
}
