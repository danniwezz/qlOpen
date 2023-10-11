using CustomerModule.Public;

namespace PricingCalculator.Application;

public class PriceCalculatorService : IPriceCalculatorService
{
	public List<ServiceCost> CalculateCustomerCost(CustomerDto customer, DateOnly? calculateUntil = null)
	{
		var costPerService = new List<ServiceCost>();
		foreach (var service in customer.AssignedServices)
		{
			//Get the total number of days the customer has had the service
			var daysWithCost = GetDaysWithCost(service.StartDate, calculateUntil ?? service.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow), service.ValidFromWeekDayNumber, service.ValidToWeekDayNumber);
			//We need to ignore the discounts that start later than the date we are calculating the costs until
			var discountCosts = service.Discounts.Where(x => x.ValidFrom < calculateUntil).Select(discount =>
			{
				//Get the total number of days the customer has had the discount on the service
				var startDate = discount.ValidFrom ?? service.StartDate;
				var endDate = discount.ValidTo ?? service.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
				var discountedDaysWithCost = GetDaysWithCost(startDate, calculateUntil.HasValue && calculateUntil.Value < endDate ? calculateUntil.Value : endDate, service.ValidFromWeekDayNumber, service.ValidToWeekDayNumber);
				return new {Cost = discountedDaysWithCost * service.Price * (1M - discount.Percentage), NumberOfDays = discountedDaysWithCost};
			}).ToList();
			var totalDiscountedDaysWithCost = discountCosts.Sum(x => x.NumberOfDays);

			//If the number of days with discount is larger than the total number of days the customer has had the service something is wrong. We shouldn't return is right?
			if(totalDiscountedDaysWithCost <= daysWithCost)
			{
				costPerService.Add(new ServiceCost(service.ServiceName, discountCosts.Sum(x => x.Cost) + ((daysWithCost - totalDiscountedDaysWithCost) * service.Price), service.Currency));
			}
		}
		return costPerService;
	}

	private int GetDaysWithCost(DateOnly startDate, DateOnly endDate, int validFromWeekDayNumber, int validToWeekDayNumber)
	{
		var totalDays = 0;
		for(var date = startDate; date < endDate; date = date.AddDays(1))
		{
			if((int)date.DayOfWeek >= validFromWeekDayNumber && (int)date.DayOfWeek <= validToWeekDayNumber)
			{
				totalDays++;
			}
		}
		return totalDays;
	}
}

public record ServiceCost(string ServiceName, decimal TotalCost, string Currency);

public interface IPriceCalculatorService
{
	List<ServiceCost> CalculateCustomerCost(CustomerDto customer, DateOnly? calculateUntil);
}
