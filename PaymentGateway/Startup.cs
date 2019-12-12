using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentGateway.Configuration;
using PaymentGateway.External;
using PaymentGateway.Services;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using System.Net.Http;
using Utils.Middleware;
using System;

namespace PaymentGateway
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services
                .AddOptions()
                .Configure<BankConfiguration>(
                    Configuration.GetSection(nameof(BankConfiguration)));

            services
                .AddSingleton<HttpClient>()
                .AddTransient<IPaymentService, PaymentService>()
                .AddSingleton<IPaymentRepository, PaymentRepository>()
                .AddTransient<IPaymentApi, PaymentApi>(CreatePaymentApi);

            services.AddMvc();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Payment Gateway API", Version = "v1" });
            });
        }

        private static PaymentApi CreatePaymentApi(IServiceProvider serviceProvider)
        {
            var bankConfig = serviceProvider.GetRequiredService<IOptions<BankConfiguration>>();
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            return new PaymentApi(httpClient, bankConfig.Value.Url);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware<RequestResponseLoggingMiddleware>(loggerFactory.CreateLogger("RequestResponseLogger"));
            app.UseMiddleware<ErrorHandlingMiddleware>(loggerFactory.CreateLogger("ExceptionHandlerLogger"));

            app.UseMvc();

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API");
            });
        }
    }
}
