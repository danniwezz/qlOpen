using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ServiceModule", Version = "v1" });
            opt.SupportNonNullableReferenceTypes();
            opt.UseAllOfToExtendReferenceSchemas();
        });
    }

    private static void UseMiddlewares(this WebApplication app)
    {
        app.UseMigrations(app.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Default sql connection string is null"));
        
        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"/swagger/v1/swagger.yaml", "Version 1.0");
                setup.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //Setup API

        app.Run();
    }
}
