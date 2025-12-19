using CartPricing.Api.Middleware;
using CartPricing.Core.Application;
using CartPricing.Core.Application.Handlers;
using CartPricing.Core.Application.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CartPricing.Api
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
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                    {
                        options.InvalidModelStateResponseFactory = context =>
                        {
                            var errors = context.ModelState
                                .Where(kvp => kvp.Value?.Errors.Count > 0)
                                .ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                                );

                            return new BadRequestObjectResult(new
                            {
                                code = "VALIDATION_ERROR",
                                message = "Request validation failed. Fix the indicated fields.",
                                errors
                            });
                        };
                    }
                );
            services
                .AddSingleton<CalculateCartTotalHandler>()
                .AddSingleton<CalculateCartTotalValidator>()
                .AddSingleton<CartCalculator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStandardErrorHandling(); // Custom Error Handling

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
