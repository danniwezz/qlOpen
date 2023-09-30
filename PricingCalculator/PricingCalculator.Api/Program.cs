using Microsoft.OpenApi.Models;

namespace PricingCalculator.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureServices();
        var app = builder.Build();
        app.UseMiddlewares();
        app.Run();
    }
}

public static partial class WebApplicationExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PricingCalculator", Version = "v1" });
            opt.SupportNonNullableReferenceTypes();
            opt.UseAllOfToExtendReferenceSchemas();
        });
        return services;
    }

    public static WebApplication UseMiddlewares(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseExceptionHandler("/Error");
        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            setup.SwaggerEndpoint($"/swagger/v1/swagger.yaml", "Version 1.0");
            setup.RoutePrefix = string.Empty;
        });
        

        app.MapGroup("api").MapGet("", () => "Hello");
        return app;
    }
}
