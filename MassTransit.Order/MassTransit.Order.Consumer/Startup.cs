using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit.AspNetCoreIntegration;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Order.Consumer.Order;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransit.Order.Consumer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(action => { action.EnableEndpointRouting = false; });

            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                return Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost/"), configurator =>
                    {
                        configurator.Username("workdata");
                        configurator.Password("workdata123!@#");
                    });
                    
                    cfg.ReceiveEndpoint("order_insert_queue1", ep =>
                    {
                        ep.ConfigureConsumer<InsertOrderConsumer>(serviceProvider);
                    });
                    
                    cfg.ReceiveEndpoint("order_insert_queue", ep =>
                    {
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        ep.ConfigureConsumer<InsertOrderConsumer>(serviceProvider);
                    });
                });
            }

            // local function to configure consumers
            void ConfigureMassTransit(IServiceCollectionConfigurator configurator)
            {
                configurator.AddConsumer<InsertOrderConsumer>();
            }

            // configures MassTransit to integrate with the built-in dependency injection
            services.AddMassTransit(CreateBus, ConfigureMassTransit);

            //services.AddMassTransit(x =>
            //{
            //    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        var host = cfg.Host(new Uri("rabbitmq://localhost/"), configurator =>
            //        {
            //            configurator.Username("workdata");
            //            configurator.Password("workdata123!@#");
            //        });

            //        cfg.ReceiveEndpoint("order_insert_queue", e => { 
            //            e.Consumer<InsertOrderConsumer>(); 
            //        });
            //    }));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}