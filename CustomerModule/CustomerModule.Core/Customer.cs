using Shared.Core;

namespace CustomerModule.Core;
public class Customer
{
	public static Customer Create(string name, long serviceId)
	{
		return new Customer
		{
			Id = IdGenerator.NewId(),
			Name = name,
			ServiceId = serviceId
		};
	}

	public static Customer Create(long id, string name, long serviceId, List<AssignedService> assignedServices)
	{
		return new Customer
		{
			Id = id,
			Name = name,
			ServiceId = serviceId,
			AssignedServices = assignedServices
		};
	}

	public void AddAssignedService(AssignedService assignedService)
	{
		if (assignedService != null && !AssignedServices.Select(x => x.Id).Contains(assignedService.Id))
		{
			AssignedServices.Add(assignedService);
		}
	}

	public void AddDiscount(long assignedServiceId, Discount discount)
	{
		if (discount != null && !AssignedServices.Single(x => x.Id == assignedServiceId).Discounts.Select(x => x.Id).Contains(discount.Id))
		{
			AssignedServices.Single(x => x.Id == assignedServiceId).Discounts.Add(discount);
		}
	}

	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public long ServiceId { get; set; }
	public ICollection<AssignedService> AssignedServices { get; set; } = new HashSet<AssignedService>();

}
