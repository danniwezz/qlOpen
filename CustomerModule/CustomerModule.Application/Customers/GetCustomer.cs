using CustomerModule.Core;
using CustomerModule.Public;
using MediatR;

namespace CustomerModule.Application.Customers;
public class GetCustomer
{
	public record Request(long CustomerId) : IRequest<CustomerDto?>;

	public class Handler : IRequestHandler<Request, CustomerDto?>
	{
		private readonly ICustomerRepository _customerRepository;

		public Handler(ICustomerRepository customerRepository)
        {
			_customerRepository = customerRepository;
		}
        public async Task<CustomerDto?> Handle(Request request, CancellationToken cancellationToken)
		{
			var customer = await  _customerRepository.SingleOrDefaultAsync(c => c.Id  == request.CustomerId, cancellationToken);
			return customer != null ? CustomerMapper.ToDto(customer) : null;
		}
	}
}
