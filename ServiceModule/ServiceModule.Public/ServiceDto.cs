namespace ServiceModule.Public;

public class ServiceDto
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public decimal Price { get; set; }
	public string Currency { get; set; } = null!;
	public int ValidFromWeekDayNumber { get; set; }
	public int ValidToWeekDayNumber { get; set; }
}
