using System;
using GreenPipes;
using MassTransit.AspNetCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Saga.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Saga.SagaInstanceService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                var sagaInstanceMachine = new SagaInstanceMachine();
                var contextFactory = new ContextFactory();
                var delegateSagaDbContextFactory = new DelegateSagaDbContextFactory<SagaInstance>(() =>
                    contextFactory.CreateDbContext(null));

                var repository = new Lazy<ISagaRepository<SagaInstance>>(() =>
                EntityFrameworkSagaRepository<SagaInstance>.CreateOptimistic(delegateSagaDbContextFactory));

                return Bus.Factory.CreateUsingRabbitMq(cfg =>
                {

                    cfg.Host(new Uri("rabbitmq://localhost/"), configurator =>
                    {
                        configurator.Username("workdata");
                        configurator.Password("workdata123!@#");
                    });

                    cfg.ReceiveEndpoint("saga_instance_state", e =>
                    {
                        //e.UseRetry(retry =>
                        //{
                        //    retry.Handle<DbUpdateConcurrencyException>();
                        //    retry.Interval(5, TimeSpan.FromMilliseconds(100));
                        //});

                        e.StateMachineSaga(sagaInstanceMachine, repository.Value);
                    });
                });
            }

            // local function to configure consumers
            void ConfigureMassTransit(IServiceCollectionConfigurator configurator)
            {
              
            }

            // configures MassTransit to integrate with the built-in dependency injection
            services.AddMassTransit(CreateBus, ConfigureMassTransit);

            //services.AddMassTransit(x =>
            //{
            //    var sagaInstanceMachine = new SagaInstanceMachine();
            //    var contextFactory = new ContextFactory();
            //    var delegateSagaDbContextFactory = new DelegateSagaDbContextFactory<SagaInstance>(() =>
            //        contextFactory.CreateDbContext(null));

            //    var repository = new Lazy<ISagaRepository<SagaInstance>>(() => 
            //    EntityFrameworkSagaRepository<SagaInstance>.CreateOptimistic(delegateSagaDbContextFactory));
            //    //var repository = new InMemorySagaRepository<SagaInstance>();
            //    // For Pessimistic
            //    //var repository = new EntityFrameworkSagaRepository<SagaInstance>.CreatePessimistic(contextFactory);
            //    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        cfg.Host(new Uri("rabbitmq://localhost/"), configurator =>
            //        {
            //            configurator.Username("workdata");
            //            configurator.Password("workdata123!@#");
            //        });

            //        cfg.ReceiveEndpoint("saga_instance_state", e =>
            //        {
            //            //e.UseRetry(retry =>
            //            //{
            //            //    retry.Handle<DbUpdateConcurrencyException>();
            //            //    retry.Interval(5, TimeSpan.FromMilliseconds(100));
            //            //});

            //            e.StateMachineSaga(sagaInstanceMachine, repository.Value);
            //        });
            //    }));

            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
