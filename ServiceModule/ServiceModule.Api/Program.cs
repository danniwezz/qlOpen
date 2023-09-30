using Microsoft.EntityFrameworkCore;
using ServiceModule.Application;
using ServiceModule.Infrastructure;
using Shared.Yuniql;

namespace ServiceModule.Api;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServices();

        var app = builder.Build();

        app.UseMiddlewares();

        app.Run();
    }

    private static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();


        builder.Services.AddDbContext<ServiceDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Default sql connection string is null")));
        builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
        builder.Services.AddScoped<IServiceUnitOfWork, ServiceUnitOfWork>();

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
