using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using PricingCalculator.Api.WebApi;
using PricingCalculator.Application;
using PricingCalculator.Core;
using PricingCalculator.Infrastructure.CustomerModuleClient;

namespace PricingCalculator.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureServices();
        var app = builder.Build();
        app.UseMiddlewares();
        app.Run();
    }
}

public static partial class WebApplicationExtensions
{
    public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PricingCalculator", Version = "v1" });
            opt.SupportNonNullableReferenceTypes();
            opt.UseAllOfToExtendReferenceSchemas();
        });
        builder.Services.AddOptions<CustomerModuleClientOptions>().Bind(builder.Configuration.GetSection(nameof(CustomerModuleClientOptions))).ValidateDataAnnotations().ValidateOnStart();
        builder.Services.AddHttpClient<ICustomerModuleClient, CustomerModuleClient>();

        builder.Services.AddScoped<ICustomerModuleClient, CustomerModuleClient>();
        return builder.Services;
    }

    public static WebApplication UseMiddlewares(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

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
        

        var apiGroup = app.MapGroup("");
        apiGroup.RegisterPriceCalculatorApi();
        return app;
    }
}
