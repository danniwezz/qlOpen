using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Shared.IntegrationTests.Helpers;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		base.ConfigureWebHost(builder);
		builder.UseEnvironment("Development");
		builder.ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.test.json"));
	}
}
