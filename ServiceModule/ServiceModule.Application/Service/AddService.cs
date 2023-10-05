using FluentValidation;
using MediatR;
using ServiceModule.Core;

namespace ServiceModule.Application.Service;
public class AddService
{
	public record Request(string Name, decimal Price, string Currency, int ValidFromWeekDayNumber, int ValidToWeekDayNumber) : IRequest;
	public class Validator : AbstractValidator<Request>
	{
		public Validator(IServiceRepository serviceRepository)
		{
			RuleFor(x => x.Price).Must(price => price >= 0).WithMessage("Price must be larger or equal to 0.");
			RuleFor(x => x.Currency).Must(currency => currency.Length == 3).WithMessage("Currency name must be 3 characters and must be provided in ISO 4217 standard.");
			RuleFor(x => x.Name).MustAsync(async (name, ct) => await serviceRepository.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower(), ct) != null).WithMessage(x => $"A service with name: {x.Name} already exist.");
			RuleFor(x => x.ValidFromWeekDayNumber).Must(number => number >= 1 && number <= 7).WithMessage("ValidFromWeekDayNumber must be within the range of [1,7].");
			RuleFor(x => x.ValidToWeekDayNumber).Must(number => number >= 1 && number <= 7).WithMessage("ValidToWeekDayNumber must be within the range of [1,7].");
		}
	}

	public class Handler : IRequestHandler<Request>
	{
		private readonly IServiceRepository _serviceRepository;
		private readonly IServiceUnitOfWork _serviceUnitOfWork;

		public Handler(IServiceRepository serviceRepository, IServiceUnitOfWork serviceUnitOfWork)
		{
			_serviceRepository = serviceRepository;
			_serviceUnitOfWork = serviceUnitOfWork;
		}
		public async Task Handle(Request request, CancellationToken cancellationToken)
		{
			_serviceRepository.Add(ServiceModule.Core.Service.Create(request.Name, request.Price, request.Currency));
			await _serviceUnitOfWork.SaveChangesAsync(cancellationToken);
		}
	}
}
