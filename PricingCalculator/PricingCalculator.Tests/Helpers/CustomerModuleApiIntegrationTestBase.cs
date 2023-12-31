﻿using CustomerModule.Api;
using CustomerModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;

namespace Shared.IntegrationTests.Helpers;

public class CustomerModuleApiIntegrationTestBase : IClassFixture<TestWebApplicationFactory<Program>>, IAsyncLifetime
{
	protected readonly TestWebApplicationFactory<Program> _factory;

	public CustomerModuleApiIntegrationTestBase(TestWebApplicationFactory<Program> factory)
	{
		_factory = factory;
	}

	public HttpClient GetHttpClient()
	{
		return _factory.CreateDefaultClient(new Uri("https://localhost:7248/"));
	}

	public Task DisposeAsync()
	{
		return Task.CompletedTask;
	}
	public async Task InitializeAsync()
	{
		using var scope = _factory.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetService<CustomerDbContext>();
		var connectionString = dbContext?.Database.GetConnectionString();
		if (connectionString == null)
		{
			return;
		}
		var respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions
		{
			TablesToIgnore = new Table[]
			{
				"__yuniql_schema_version"
			}
		});

		await respawner.ResetAsync(connectionString);
	}
}