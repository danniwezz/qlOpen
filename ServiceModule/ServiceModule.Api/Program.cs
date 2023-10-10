using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ServiceModule.Api.WebApi;
using ServiceModule.Application.Service;
using ServiceModule.Core;
using ServiceModule.Infrastructure;
using Shared.FluentValidation;
using Shared.Yuniql;

namespace ServiceModule.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServices();

        var app = builder.Build();

        app.UseMiddlewares();

        app.Run();
    }

}
public static partial class WebApplicationExtensions 
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();


        builder.Services.AddDbContext<ServiceDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Default sql connection string is null")));
        builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
        builder.Services.AddScoped<IServiceUnitOfWork, ServiceUnitOfWork>();


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ServiceModule", Version = "v1" });
            opt.SupportNonNullableReferenceTypes();
            opt.UseAllOfToExtendReferenceSchemas();
        });

        builder.Services.AddValidatingMediator(typeof(AddService));

    }

    public static void UseMiddlewares(this WebApplication app)
    {
        app.UseMigrations(app.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Default sql connection string is null"), "ServiceMigrations");
        
        if (app.Environment.IsDevelopment())
        {
           app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    await context.Response.WriteAsJsonAsync(new { contextFeature?.Error.Message });
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

        var apiGroup = app.MapGroup("").WithOpenApi();
        apiGroup.RegisterServiceApi();


        app.Run();
    }
}
