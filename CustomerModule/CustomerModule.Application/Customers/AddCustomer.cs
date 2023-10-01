using CustomerModule.Core;
using FluentValidation;
using MediatR;

namespace CustomerModule.Application.Customers;
public class AddCustomer
{
	public record Request(string Name, List<AssignedServiceDto> AssignedServices, List<DiscountDto> Discounts) : IRequest;


	public class Validator : AbstractValidator<Request>
	{
        public Validator()
        {
            
        }
    }

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
			var customer = Customer.Create(request.Name);
			foreach (var assignedServiceDto in request.AssignedServices)
			{
				var assignedService = AssignedService.Create(assignedServiceDto.ServiceId, assignedServiceDto.ServiceName, customer.Id);
				customer.AddAssignedService(assignedService);

				foreach (var discount in request.Discounts.Where(x => x.ServiceId == assignedService.ServiceId))
				{
					customer.AddDiscount(Discount.Create(assignedService.Id, discount.Percentage, discount.ValidFrom, discount.ValidTo));
				}
			}

			_customerRepository.Add(customer);
			await _customerUnitOfWork.SaveChangesAsync(cancellationToken);
		}
	}
}

public record AddCustomerRequestDto(string Name, List<AssignedServiceDto> AssignedServices, List<DiscountDto> Discounts);
public record AssignedServiceDto(long ServiceId, string ServiceName);
public record DiscountDto(long ServiceId, decimal Percentage, DateOnly? ValidFrom, DateOnly? ValidTo);