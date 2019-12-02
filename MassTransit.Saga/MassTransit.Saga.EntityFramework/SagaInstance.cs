using Automatonymous;
using System;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MassTransit.Saga.EntityFramework.Event;

namespace MassTransit.Saga.EntityFramework
{
    public class SagaInstance : SagaStateMachineInstance
    {
        public SagaInstance()
        {
        }

        public int CurrentState { get; set; }
        public string ServiceName { get; set; }
        public Guid CorrelationId { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class SagaInstanceMachine : MassTransitStateMachine<SagaInstance>
    {
        public SagaInstanceMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Cancel);
            Event(() => InsertSagaInstance,
                x => x.CorrelateById(c => c.Message.CorrelationId));
            Event(() => CancelSagaInstance,
                x => x.CorrelateById(c => c.Message.CorrelationId));     

            During(Initial,
                When(InsertSagaInstance)
                    .Execute(context => new InsertSagaInstanceActivity())
                    .TransitionTo(Submitted));

            During(Submitted,
                When(CancelSagaInstance)
                    .Execute(context => new CancelSagaInstanceActivity())
                    .TransitionTo(Cancel));
        }

        public State Submitted { get; set; }

        public State Cancel { get; set; }

        public Event<InsertSagaInstance> InsertSagaInstance { get; set; }

        public Event<ICancelSagaInstance> CancelSagaInstance { get; set; }
    }


    public class SagaInstanceMap : IEntityTypeConfiguration<SagaInstance>
    {
        public void Configure(EntityTypeBuilder<SagaInstance> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.CorrelationId);
            entityTypeBuilder.Property(t => t.CorrelationId)
                .ValueGeneratedNever();
            entityTypeBuilder.Property(x => x.ServiceName).HasMaxLength(40);
            entityTypeBuilder.Property(x => x.CurrentState);
            entityTypeBuilder.Property(x => x.RowVersion).IsRowVersion();
   
        }
    }
}