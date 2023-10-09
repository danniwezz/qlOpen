﻿using CustomerModule.Core;
using FluentValidation;
using MediatR;

namespace CustomerModule.Application.Customers;
public class AddCustomer
{
	public record Request(string Name, List<AssignedService> AssignedServices) : IRequest<long>;

	public class Validator : AbstractValidator<Request>
	{
		public Validator(ICustomerRepository customerRepository)
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Name).MustAsync(async (name, ct) => await customerRepository.FirstOrDefaultAsync(x => x.Name == name, ct) == null).WithMessage(x => $"A customer with name: {x.Name} already exist.");
			RuleFor(x => x.AssignedServices).Must(services => services.Select(s => s.ServiceId).Distinct().Count() == services.Count).WithMessage("A serviceId can only be assigned once per customer.");
			RuleFor(x => x.AssignedServices).Must(services => services.Select(s => s.ServiceName).Distinct().Count() == services.Count).WithMessage("A serviceName can only be assigned once per customer.");
			RuleForEach(x => x.AssignedServices).Must(service =>
			{
				foreach (var discount in service.Discounts)
				{
					if (discount.ValidFrom > discount.ValidTo)
					{
						return false;
					}
				}
				return true;

			}).WithMessage(x => $"ValidFrom must come before ValidTo");
			RuleForEach(x => x.AssignedServices).Must(service =>
			{
				foreach (var discount in service.Discounts)
				{
					if (!(discount.ValidFrom == null && discount.ValidTo == null) && !(discount.ValidFrom != null && discount.ValidTo != null))
					{
						return false;
					}
				}
				return true;

			}).WithMessage(x => "ValidFrom and ValidTo must either both be null or both have values.");
			RuleForEach(x => x.AssignedServices).Must(service =>
			{
				if (service.Discounts.Any(x => x.ValidFrom == null && x.ValidTo == null) && service.Discounts.Count > 1)
				{
					return false;
				}
				return true;

			}).WithMessage("If a discount with no startDate or endDate exist, then there should only exist one discount.");
			RuleForEach(x => x.AssignedServices).Must(service =>
			{
				foreach (var discount in service.Discounts)
				{
					if (service.Discounts.Count >= 2 && !service.Discounts.Where(x => x != discount).Any(other => (discount.ValidFrom < other.ValidFrom && other.ValidFrom < discount.ValidFrom) //The "other discount"s startDate is contained in the "discount"s period
					|| (other.ValidFrom < discount.ValidFrom && other.ValidTo > discount.ValidFrom)                         //The "discount"s startDate is contained in the "other discount"s period
					|| (other.ValidFrom < discount.ValidFrom && discount.ValidTo < other.ValidTo)                               //The "discounts" period is contained in the "other discount"s period
					|| (discount.ValidFrom < other.ValidFrom && other.ValidTo < discount.ValidTo)                               //The "other discount"s period is contained in the discounts period
					))
					{
						return false;
					}
				}

				return true;
			}).WithMessage("Periods can not overlap.");
		}
	}

	public class Handler : IRequestHandler<Request, long>
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly ICustomerUnitOfWork _customerUnitOfWork;

		public Handler(ICustomerRepository customerRepository, ICustomerUnitOfWork customerUnitOfWork)
		{
			_customerRepository = customerRepository;
			_customerUnitOfWork = customerUnitOfWork;
		}
		public async Task<long> Handle(Request request, CancellationToken cancellationToken)
		{
			var customer = Customer.Create(request.Name);
			foreach (var assignedServiceDto in request.AssignedServices)
			{
				var assignedService = AssignedService.Create(assignedServiceDto.ServiceId, assignedServiceDto.ServiceName, customer.Id,
					assignedServiceDto.Price, assignedServiceDto.Currency, assignedServiceDto.StartDate, assignedServiceDto.EndDate, assignedServiceDto.ValidFromWeekDayNumber, assignedServiceDto.ValidToWeekDayNumber);

				foreach (var discount in request.AssignedServices.Single(x => x.ServiceId == assignedService.ServiceId).Discounts.ToList())
				{
					assignedService.AddDiscount(Discount.Create(assignedService.Id, discount.Percentage, discount.ValidFrom, discount.ValidTo));
				}

				customer.AddAssignedService(assignedService);
			}

			_customerRepository.Add(customer);
			await _customerUnitOfWork.SaveChangesAsync(cancellationToken);
			return customer.Id;
		}
	}
}