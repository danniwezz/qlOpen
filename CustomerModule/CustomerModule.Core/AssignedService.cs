using Shared.Core;

namespace CustomerModule.Core;
public class AssignedService
{

     public static AssignedService Load(long ServiceId, long serviceId, string serviceName, long customerId)
    {
        return new AssignedService
        {
            Id = serviceId,
            ServiceId = serviceId,
            ServiceName = serviceName,
            CustomerId = customerId
        };
    }

    public static AssignedService Create(long serviceId, string serviceName, long customerId)
    {
        return new AssignedService
        {
            Id = IdGenerator.NewId(),
            ServiceId = serviceId,
            ServiceName = serviceName,
            CustomerId = customerId
        };
    }
    public long Id { get; set; }
    public long ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public long CustomerId { get; set; }
    public ICollection<Discount> Discounts { get; set; } = new HashSet<Discount>();
}
