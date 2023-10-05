using CustomerModule.Application.Customers;
using CustomerModule.Public;
using Riok.Mapperly.Abstractions;

namespace CustomerModule.Api.Mappers;

[Mapper]
public static partial class CustomerRequstMapper
{
	public static partial AddCustomer.Request FromDto(AddCustomerRequestDto request);

}
