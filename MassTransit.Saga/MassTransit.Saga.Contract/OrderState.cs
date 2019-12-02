using Automatonymous;
using GreenPipes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Saga.Contract
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public State CurrentState { get; set; }
    }

    public interface SubmitOrder
    {
        Guid OrderId { get; }
    }

    public interface OrderAccepted
    {
        Guid OrderId { get; }
    }

    public class OrderAcceptedActivity : Activity<OrderState, OrderAccepted>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            Console.WriteLine("Accept");
        }

        public Task Execute(BehaviorContext<OrderState, OrderAccepted> context, Behavior<OrderState, OrderAccepted> next)
        {
            Console.WriteLine("Execute");
            return next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderAccepted, TException> context, Behavior<OrderState, OrderAccepted> next) where TException : Exception
        {
            Console.WriteLine("Faulted");
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            Console.WriteLine("Probe");
        }
    }

    public class SubmitOrderActivity : Activity<OrderState, SubmitOrder>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            Console.WriteLine("Accept");
        }

        public Task Execute(BehaviorContext<OrderState, SubmitOrder> context, Behavior<OrderState, SubmitOrder> next)
        {
            Console.WriteLine("Execute");
            return next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, SubmitOrder, TException> context, Behavior<OrderState, SubmitOrder> next) where TException : Exception
        {
            Console.WriteLine("Faulted");
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            Console.WriteLine("Probe");
        }
    }

    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            Initially(
                When(SubmitOrder)
                    .Execute(context => new SubmitOrderActivity())
                    .TransitionTo(Submitted),
                When(OrderAccepted)
                    .Execute(context => new OrderAcceptedActivity())
                    .TransitionTo(Accepted));

            During(Submitted,
                When(OrderAccepted)
                    .Execute(context => new OrderAcceptedActivity())
                    .TransitionTo(Accepted));

            During(Accepted,
                Ignore(SubmitOrder));
        }

        public State Submitted { get; private set; }
        public State Accepted { get; private set; }

        public Event<SubmitOrder> SubmitOrder { get; private set; }

        public Event<OrderAccepted> OrderAccepted { get; private set; }
    }
}
