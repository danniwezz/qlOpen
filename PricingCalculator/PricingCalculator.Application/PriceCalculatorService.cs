using CustomerModule.Public;

namespace PricingCalculator.Application;

public class PriceCalculatorService : IPriceCalculatorService
{
	public List<ServiceCost> CalculateCustomerCost(CustomerDto customer)
	{
		var costPerService = new List<ServiceCost>();
		foreach (var service in customer.AssignedServices)
		{
			//Get the total number of days the customer has had the service
			var daysWithCost = GetDaysWithCost(service.StartDate, service.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow), service.ValidFromWeekDayNumber, service.ValidToWeekDayNumber);
			var discountCosts = service.Discounts.Select(discount =>
			{
				//Get the total number of days the customer has had the discount on the service
				var discountedDaysWithCost = GetDaysWithCost(discount.ValidFrom ?? service.StartDate, discount.ValidTo ?? service.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow), service.ValidFromWeekDayNumber, service.ValidToWeekDayNumber);
				return new {Cost = discountedDaysWithCost * service.Price * discount.Percentage, NumberOfDays = discountedDaysWithCost};
			});
			var totalDiscountedDaysWithCost = discountCosts.Sum(x => x.NumberOfDays);

			//If the number of days with discount is larger than the total number of days the customer has had the service something is wrong. We shouldn't return is right?
			if(totalDiscountedDaysWithCost <= daysWithCost)
			{
				costPerService.Add(new ServiceCost(service.ServiceName, discountCosts.Sum(x => x.Cost) + ((daysWithCost - totalDiscountedDaysWithCost) * service.Price)));
			}
		}
		return costPerService;
	}

	private int GetDaysWithCost(DateOnly startDate, DateOnly endDate, int validFromWeekDayNumber, int validToWeekDayNumber)
	{
		var totalDays = 0;
		for(var date = startDate; startDate < endDate; date.AddDays(1))
		{
			if(validFromWeekDayNumber <= (int)date.DayOfWeek && validToWeekDayNumber <= (int)date.DayOfWeek)
			{
				totalDays++;
			}
		}
		return totalDays;
	}
}

public record ServiceCost(string ServiceName, decimal TotalCost);

public interface IPriceCalculatorService
{
	List<ServiceCost> CalculateCustomerCost(CustomerDto customer);
}
