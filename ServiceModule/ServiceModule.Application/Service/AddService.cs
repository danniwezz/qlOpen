using MediatR;
using ServiceModule.Core;

namespace ServiceModule.Application.Service;
public class AddService
{
	public record Request(string Name, decimal Price, string Currency) : IRequest;

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
