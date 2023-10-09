using Shared.Core;

namespace CustomerModule.Core;
public class Customer
{
	public static Customer Create(string name)
	{
		return new Customer
		{
			Id = IdGenerator.NewId(),
			Name = name,
		};
	}

	public static Customer Load(long id, string name, List<AssignedService> assignedServices)
	{
		return new Customer
		{
			Id = id,
			Name = name,
			AssignedServices = assignedServices
		};
	}

	public void AddAssignedService(AssignedService assignedService)
	{
		if (assignedService != null || !AssignedServices.Select(x => x.Id).Contains(assignedService.Id))
		{
			AssignedServices.Add(assignedService);
			return;
		}
		throw new Exception("Assigned service is null or already exist.");
	}

	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public ICollection<AssignedService> AssignedServices { get; set; } = new HashSet<AssignedService>();

}
