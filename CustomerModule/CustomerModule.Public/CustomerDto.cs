namespace CustomerModule.Public;

public class CustomerDto
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public ICollection<AssignedServiceDto> AssignedServices { get; set; } = new HashSet<AssignedServiceDto>();
}
