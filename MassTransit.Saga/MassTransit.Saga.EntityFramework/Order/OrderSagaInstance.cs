using Automatonymous;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Saga.EntityFramework.Order
{
    public class OrderSagaInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public Guid OrderId { get; set; }

        public int CurrentState { get; set; }

        public byte[] RowVersion { get; set; }
    }

    public class OrderSagaInstanceMachine : MassTransitStateMachine<OrderSagaInstance>
    {
        public OrderSagaInstanceMachine()
        {

        }
    }


    public class OrderSagaInstanceMap : IEntityTypeConfiguration<OrderSagaInstance>
    {
        public void Configure(EntityTypeBuilder<OrderSagaInstance> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.CorrelationId);
            entityTypeBuilder.Property(t => t.CorrelationId)
                .ValueGeneratedNever();
            entityTypeBuilder.Property(x => x.CurrentState);
            entityTypeBuilder.Property(x => x.RowVersion).IsRowVersion();

        }
    }
}
