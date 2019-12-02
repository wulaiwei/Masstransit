using System;
using System.Data.SqlClient;
using GreenPipes;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.Saga.Contract;
using MassTransit.Saga.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransit.Saga.OrderService
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

            services.AddMassTransit(x =>
            {
                var sagaInstanceMachine = new SagaInstanceMachine();
                var contextFactory = new ContextFactory();
                var s= contextFactory.CreateDbContext(null);
                var delegateSagaDbContextFactory = new DelegateSagaDbContextFactory<SagaInstance>(() =>
                    contextFactory.CreateDbContext(null));

                var repository = EntityFrameworkSagaRepository<SagaInstance>.CreateOptimistic(delegateSagaDbContextFactory);

                // For Pessimistic
                //var repository = new EntityFrameworkSagaRepository<SagaInstance>.CreatePessimistic(contextFactory);
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost/"), configurator =>
                    {
                        configurator.Username("workdata");
                        configurator.Password("workdata123!@#");
                    });

                    cfg.ReceiveEndpoint("saga_instance_state", e =>
                    {
                        e.UseRetry(x =>
                        {
                            x.Handle<DbUpdateConcurrencyException>();
                            x.Handle<DbUpdateException>(y => y.InnerException is SqlException e && e.Number == 2627); // This is the SQLServer error code for duplicate key, if you are using another Relational Db, the code might be different
                            x.Interval(5, TimeSpan.FromMilliseconds(100));

                            
                        }); // Add the retry middleware for optimistic concurrency

                        e.StateMachineSaga(sagaInstanceMachine, repository);
                    });
            }));

        });
        }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
}
