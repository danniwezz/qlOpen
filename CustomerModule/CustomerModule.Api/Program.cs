using CustomerModule.Application;
using CustomerModule.Core;
using CustomerModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Yuniql;

namespace CustomerModule.Api;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServices();

        var app = builder.Build();

        app.UseMiddlewares();
    }

    private static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CustomerDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Default sql connection string is null")));

        builder.Services.AddAuthorization();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static void UseMiddlewares(this WebApplication app)
    {
        app.UseMigrations(app.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Default sql connection string is null"));
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //Setup API

        app.Run();
    }
}
