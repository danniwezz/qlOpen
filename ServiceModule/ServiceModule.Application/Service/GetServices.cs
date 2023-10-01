using MediatR;
using ServiceModule.Core;

namespace ServiceModule.Application.Service;
public class GetServices
{
	public record Request : IRequest<List<ServiceModule.Core.Service>>;

	public class Handler : IRequestHandler<Request, List<ServiceModule.Core.Service>>
	{
		private readonly IServiceRepository _serviceRepository;

		public Handler(IServiceRepository serviceRepository)
        {
			_serviceRepository = serviceRepository;
		}
        public async Task<List<ServiceModule.Core.Service>> Handle(Request request, CancellationToken cancellationToken)
		{
			return (await _serviceRepository.QueryAsync(x => true, cancellationToken)).ToList();
		}
	}
}
