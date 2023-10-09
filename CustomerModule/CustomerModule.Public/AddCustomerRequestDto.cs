namespace CustomerModule.Public;
public record AddCustomerRequestDto(string Name, List<AssignedServiceDto> AssignedServices);