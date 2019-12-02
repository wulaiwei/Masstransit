using Automatonymous;
using GreenPipes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Order.Automatonymous
{
    public class OrderState
    {
        public State CurrentState { get; set; }

        public int CompositeStatus { get; set; }
        public Guid OrderId { get; set; }

        public DateTime CreateDateTime { get; set; }

    }

    public class OrderStockActivity : Activity<OrderState, OrderStock>
    {
        public void Accept(StateMachineVisitor visitor)
        {
    
        }

        public Task Execute(BehaviorContext<OrderState, OrderStock> context, Behavior<OrderState, OrderStock> next)
        {
            return next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderStock, TException> context, Behavior<OrderState, OrderStock> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
   
        }
    }

    public class OrderSubmissionActivity : Activity<OrderState, OrderSubmitted>
    {
        public void Accept(StateMachineVisitor visitor)
        {

        }

        public Task Execute(BehaviorContext<OrderState, OrderSubmitted> context, Behavior<OrderState, OrderSubmitted> next)
        {
            context.Instance.CompositeStatus = 1;
            return next.Execute(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderSubmitted, TException> context, Behavior<OrderState, OrderSubmitted> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {

        }
    }

    public class OrderStateMachine : AutomatonymousStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            During(Initial,
                When(提交订单)
                    .Execute(context => new OrderSubmissionActivity())
                    .TransitionTo(已提交),
                When(OrderCancellationRequested)
                    .TransitionTo(已取消));

            During(已提交,
                When(扣除库存)
                    .Execute(context => new OrderStockActivity())
                    .TransitionTo(同步库存),
                When(OrderCancellationRequested)
                    .TransitionTo(已取消));

            WhenEnter(同步库存,
                context => context.TransitionTo(待支付));

            During(待支付,
                When(订单支付).TransitionTo(已支付),
                When(OrderCancellationRequested)
                    .TransitionTo(已取消));

            During(已支付,
                When(订单收货)
                    .TransitionTo(已完成),
                When(OrderCancellationRequested)
                    .TransitionTo(已取消)
              );
            DuringAny(When(T1).Then(context =>
            {
                var s = context;
            }));

            CompositeEvent(() => T1, x => x.CompositeStatus, 提交订单, 扣除库存, OrderCancellationRequested);
        }

        public State 已提交 { get; private set; }
        public State 同步库存 { get; private set; }
        public State 待支付 { get; private set; }
        public State 已支付 { get; private set; }
        public State 已完成 { get; private set; }
        public State 已取消 { get; private set; }

        public State Accepted { get; private set; }

        public Event<OrderSubmitted> 提交订单 { get; private set; }

        public Event<OrderStock> 扣除库存 { get; private set; }

        public Event<OrderPay> 订单支付 { get; private set; }

        public Event<OrderReceipt> 订单收货{ get; private set; }

        public Event OrderCancellationRequested { get; private set; }

        public Event T1 { get; private set; }
    }
    
    public class OrderPay
    {
        public string OrderId { get; set; }

        public double TotalPrice { get; set; }
    }

    public class OrderReceipt
    {
        public string OrderId { get; set; }
    }

    public class OrderStock
    {
        public string OrderId { get; set; }
    }

    public class OrderSubmitted
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime ReceiveTimestamp { get; set; }
    }
}
