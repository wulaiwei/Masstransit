using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Saga.EntityFramework.Order
{
    public class Order
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public double TotalPrice { get; set; }

        public List<OrderItem> Items { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .ValueGeneratedNever();
            entityTypeBuilder.Property(x => x.UserName).HasMaxLength(40);
        
        }
    }
}
