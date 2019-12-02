using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit.AspNetCoreIntegration;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Request.Consumer.Consumer;
using MassTransit.Request.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransit.Request.Consumer
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

            //IBusControl CreateBus(IServiceProvider serviceProvider)
            //{
            //    return Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        cfg.Host(new Uri("rabbitmq://localhost/"), configurator =>
            //        {
            //            configurator.Username("workdata");
            //            configurator.Password("workdata123!@#");
            //        });

            //        cfg.ReceiveEndpoint("orderStatus_check_queue", ep =>
            //        {
            //            ep.UseMessageRetry(r => r.Interval(2, 100));

            //            ep.ConfigureConsumer<CheckOrderStatusConsumer>(serviceProvider);
            //        });

              

            //    });
            //}

            //// local function to configure consumers
            //void ConfigureMassTransit(IServiceCollectionConfigurator configurator)
            //{
            //    configurator.AddConsumer<CheckOrderStatusConsumer>();

            //    configurator.AddRequestClient<CheckOrderStatus>();
            //}

            //// configures MassTransit to integrate with the built-in dependency injection
            //services.AddMassTransit(CreateBus, ConfigureMassTransit);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CheckOrderStatusConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost/"), configurator =>
                    {
                        configurator.Username("workdata");
                        configurator.Password("workdata123!@#");
                    });

                    cfg.ConfigureEndpoints(provider);
                }));

                x.AddRequestClient<CheckOrderStatus>();
            });
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
