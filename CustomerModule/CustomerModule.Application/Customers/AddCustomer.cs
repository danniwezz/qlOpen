using CustomerModule.Core;
using FluentValidation;
using MediatR;

namespace CustomerModule.Application.Customers;
public class AddCustomer
{
	public record Request(string Name, List<AssignedService> AssignedServices, List<Discount> Discounts) : IRequest;

	public class Validator : AbstractValidator<Request>
	{
        public Validator(ICustomerRepository customerRepository)
        {
            RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Name).MustAsync(async (name, ct) => await customerRepository.FirstOrDefaultAsync(x => x.Name == name, ct) == null).WithMessage(x => $"A customer with name: {x.Name} already exist.");
			RuleFor(x => x.AssignedServices).Must(services => services.Select(s => s.ServiceId).Distinct().Count() != services.Count).WithMessage("A serviceId can only be assigned once per customer.");
			RuleFor(x => x.AssignedServices).Must(services => services.Select(s => s.ServiceName).Distinct().Count() != services.Count).WithMessage("A serviceName can only be assigned once per customer.");
			RuleForEach(x => x.Discounts).Must(discount => discount.ValidFrom < discount.ValidTo).WithMessage(x => $"ValidFrom must be before ValidTo");
			//Check if every discount has a start and an end date or nothing. Else throw error
			RuleFor(x => x.Discounts).Must(discounts =>
			{
				return false;
			}).WithMessage("If a discount with no startDate or endDate exist, then there should only exist one discount.")
			RuleFor(x => x.Discounts).Must(discounts =>
			{
				foreach (var discount in discounts)
				{
					if (!discounts.Where(x => x != discount).Any(other => (discount.ValidFrom < other.ValidFrom && other.ValidFrom < discount.ValidFrom ) //The "other discount"s startDate is contained in the "discount"s period
					|| (other.ValidFrom < discount.ValidFrom && other.ValidTo > discount.ValidFrom )							//The "discount"s startDate is contained in the "other discount"s period
					|| (other.ValidFrom < discount.ValidFrom && discount.ValidTo < other.ValidTo)								//The "discounts" period is contained in the "other discount"s period
					|| (discount.ValidFrom < other.ValidFrom && other.ValidTo < discount.ValidTo)								//The "other discount"s period is contained in the discounts period
					))
					{
						return false;
					}
				}
				return true;
			}).WithMessage("Periods can not overlap.");
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

				foreach (var discount in request.Discounts.Where(x => x.AssignedServiceId == assignedService.Id))
				{
					customer.AddDiscount(Discount.Create(assignedService.Id, discount.Percentage, discount.ValidFrom, discount.ValidTo));
				}
			}

			_customerRepository.Add(customer);
			await _customerUnitOfWork.SaveChangesAsync(cancellationToken);
		}
	}
}