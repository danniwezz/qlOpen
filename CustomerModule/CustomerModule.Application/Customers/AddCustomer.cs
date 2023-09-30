using CustomerModule.Core;
using MediatR;

namespace CustomerModule.Application.Customers;
public class AddCustomer
{
	public record Request(string Name) : IRequest;

	public class Handler : IRequestHandler<Request>
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly ICustomerUnitOfWork _customerUnitOfWork;

		public Handler(ICustomerRepository customerRepository, ICustomerUnitOfWork customerUnitOfWork)
        {
			_customerRepository = customerRepository;
			_customerUnitOfWork = customerUnitOfWork;
		}
        public async Task Handle(Request request, CancellationToken cancellationToken)
		{
			_customerRepository.Add(Customer.Create(request.Name));
			await _customerUnitOfWork.SaveChangesAsync(cancellationToken);
		}
	}
}
