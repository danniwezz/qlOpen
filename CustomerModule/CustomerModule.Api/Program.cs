using CustomerModule.Api.WebApi;
using CustomerModule.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Yuniql;
using Shared.FluentValidation;
using CustomerModule.Application.Customers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using CustomerModule.Core;
using System.Reflection;

namespace CustomerModule.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServices();

        var app = builder.Build();

        app.UseMiddlewares();
    }

}

public static partial class WebApplicationExtensions 
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CustomerDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Default sql connection string is null")));

        builder.Services.AddAuthorization();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ICustomerUnitOfWork, CustomerUnitOfWork>();
      
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerModule.Api", Version = "v1" });
            opt.SupportNonNullableReferenceTypes();
            opt.UseAllOfToExtendReferenceSchemas();
        });
       
		builder.Services.AddValidatingMediator(typeof(AddCustomer));
    }

    public static void UseMiddlewares(this WebApplication app)
    {
        var assembly = typeof(Program).GetTypeInfo().Assembly;
        var c = assembly.Location.Replace($"{assembly.GetName().Name}.dll", "");
        app.UseMigrations(app.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Default sql connection string is null"), assembly.Location.Replace(assembly.FullName, ""));
        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    await context.Response.WriteAsJsonAsync(new { contextFeature?.Error });
                    await context.Response.CompleteAsync();
                });
            }); 
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"/swagger/v1/swagger.yaml", "Version 1.0");
                setup.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var apiGroup = app.MapGroup("");
        apiGroup.RegisterICustomerApi();

        app.Run();
    }
}
