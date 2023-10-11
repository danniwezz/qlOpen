using Shared.Core;

namespace CustomerModule.Core;
public class AssignedService
{
	public static AssignedService Create(long serviceId, string serviceName, long customerId, decimal price, string currency, DateOnly startDate, DateOnly? endDate, int validFromWeekDayNumber, int validToWeekDayNumber)
	{
		return new AssignedService
		{
			Id = IdGenerator.NewId(),
			ServiceId = serviceId,
			ServiceName = serviceName,
			CustomerId = customerId,
			Price = price,
			Currency = currency,
			StartDate = startDate,
			EndDate = endDate,
			ValidFromWeekDayNumber = validFromWeekDayNumber,
			ValidToWeekDayNumber = validToWeekDayNumber,
		};
	}

	public void AddDiscount(Discount discount)
	{
		if (discount == null || Discounts.Select(x => x.Id).Contains(discount.Id))
		{
			throw new Exception("Discount is null or already exist.");

		}

		Discounts.Add(discount);

	}
	public long Id { get; set; }
	public long ServiceId { get; set; }
	public string ServiceName { get; set; } = null!;
	public long CustomerId { get; set; }
	public decimal Price { get; set; }
	public string Currency { get; set; } = null!;
	public DateOnly StartDate { get; set; }
	public DateOnly? EndDate { get; set; }
	public int ValidFromWeekDayNumber { get; set; }
	public int ValidToWeekDayNumber { get; set; }
	public ICollection<Discount> Discounts { get; set; } = new HashSet<Discount>();
}
