using CustomerModule.Public;
using Riok.Mapperly.Abstractions;

namespace CustomerModule.Core;

[Mapper]
public static partial class CustomerMapper
{
	public static partial CustomerDto ToDto(Customer customer);
}

