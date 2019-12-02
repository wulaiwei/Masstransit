using Automatonymous;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GreenPipes;

namespace MassTransit.Saga.EntityFramework.Event
{
    public interface ICancelSagaInstance
    {
        Guid CorrelationId { get; set; }
        
        string ServiceName { get; set; }
    }

    public class CancelSagaInstanceActivity : Activity<SagaInstance, ICancelSagaInstance>
    {
        public void Probe(ProbeContext context)
        {
          
        }

        public void Accept(StateMachineVisitor visitor)
        {
     
        }

        public Task Execute(BehaviorContext<SagaInstance, ICancelSagaInstance> context, Behavior<SagaInstance, ICancelSagaInstance> next)
        {
            return next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<SagaInstance, ICancelSagaInstance, TException> context, Behavior<SagaInstance, ICancelSagaInstance> next) where TException : Exception
        {
            return next.Execute(context);
        }
    }
}
