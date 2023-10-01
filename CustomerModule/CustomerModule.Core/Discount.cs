using Shared.Core;
using System.Runtime.InteropServices;

namespace CustomerModule.Core;
public class Discount
{
    public static Discount Create(long assignedServiceId, decimal percentage, DateOnly? validFrom, DateOnly? validTo)
    {
        return new Discount
        {
            Id = IdGenerator.NewId(),
            AssignedServiceId = assignedServiceId,
            Percentage = percentage,
            ValidFrom = validFrom,
            ValidTo = validTo
        };
    }

    public long Id { get; set; }
    public long AssignedServiceId { get; set; }
    public decimal Percentage { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
