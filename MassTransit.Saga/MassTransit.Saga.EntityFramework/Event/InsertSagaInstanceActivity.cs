using Automatonymous;
using GreenPipes;
using System;
using System.Threading.Tasks;

namespace MassTransit.Saga.EntityFramework.Event
{
    public interface InsertSagaInstance
    {
        string ServiceName { get; set; }
        Guid CorrelationId { get; set; }
    }

    public class InsertSagaInstanceActivity : Activity<SagaInstance, InsertSagaInstance>
    {
        public void Accept(StateMachineVisitor visitor)
        {

        }

        public Task Execute(BehaviorContext<SagaInstance, InsertSagaInstance> context, Behavior<SagaInstance, InsertSagaInstance> next)
        {
            return next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<SagaInstance, InsertSagaInstance, TException> context, Behavior<SagaInstance, InsertSagaInstance> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {

        }
    }
}
