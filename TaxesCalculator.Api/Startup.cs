using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using Taxes.Services;
using Taxes.Services.Proxy;
using TaxesCalculator.Core.Proxy.Serialization;
using TaxesCalculator.Core.Proxy.Serialization.Json;

namespace TaxesCalculator.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IObjectSerializer>(new JsonObjectSerializer(new JsonSerializerSettings()));
            services.AddHttpProxy<ITaxCalculatorProxy, TaxCalculatorProxy>("TaxRateApiBaseUrl");
            services.AddScoped<ITaxCalculatorService, TaxJarCalculatorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

public static class ConfigureServicesExtension
{

    public static IHttpClientBuilder AddHttpProxy<TClient, TImplementation>(
           this IServiceCollection services,
           string baseAddressKey = null)
           where TClient : class
           where TImplementation : class, TClient
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IObjectSerializer>(new JsonObjectSerializer(new JsonSerializerSettings()));

        var builder = services
            .AddHttpClient<TClient, TImplementation>()
            .ConfigureHttpClient((provider, client) =>
            {
                if (!string.IsNullOrWhiteSpace(baseAddressKey))
                {
                    IConfiguration config = provider.GetService<IConfiguration>();

                    string baseAddress = GetBaseAddress(baseAddressKey, config);

                    if (string.IsNullOrWhiteSpace(baseAddress))
                    {
                        throw new ArgumentNullException(baseAddressKey);
                    }

                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Add("Authorization", "Token token=\"e950aa12c94d74b29873a5db8536068e\"");
                }
            });
        return builder;
    }

    private static string GetBaseAddress(string baseAddressKey, IConfiguration config)
    {
        return config.GetSection("AppSettings").GetValue<string>(baseAddressKey);
    }
}
