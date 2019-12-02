using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MassTransit.Saga.EntityFramework
{
    public class ContextFactory : IDesignTimeDbContextFactory<SagaInstanceContext>
    {
        public SagaInstanceContext CreateDbContext(string[] args)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<SagaInstanceContext>();

            dbContextOptionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=SagaInstance;Integrated Security=True;",
                m =>
                {
                    var executingAssembly = typeof(ContextFactory).GetTypeInfo().Assembly;
                    m.MigrationsAssembly(executingAssembly.GetName().Name);
                });

            return new SagaInstanceContext(dbContextOptionsBuilder.Options);
        }
    }
}
