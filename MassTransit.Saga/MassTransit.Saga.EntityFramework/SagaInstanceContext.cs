using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Saga.EntityFramework.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Saga.EntityFramework
{
    public class SagaInstanceContext : SagaDbContext<SagaInstance, SagaInstanceMap>
    {
        public DbSet<Order.Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public SagaInstanceContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
