using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Saga.EntityFramework.Order
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid ProductId{ get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }
    }

    public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .ValueGeneratedNever();
            entityTypeBuilder.Property(x => x.ProductName).HasMaxLength(40);

            entityTypeBuilder.HasOne<Order>(x => x.Order)
                .WithMany(g => g.Items)
                .HasForeignKey(s => s.OrderId);
        }
    }
}
