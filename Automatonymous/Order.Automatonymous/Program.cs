using Automatonymous;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Order.Automatonymous
{
    class Program
    {
        static  async Task Main(string[] args)
        {
            //var relationship = new Relationship();
            //var machine = new RelationshipStateMachine();

            ////machine.RaiseEvent(relationship, machine.Hello);

            //var person = new Person { Name = "Joe" };
            //machine.RaiseEvent(relationship, machine.Hello, person);

            //Console.WriteLine("Hello World!");

            
            var instance = new OrderState();
            var machine = new OrderStateMachine();

            var orderSubmitted = new OrderSubmitted
            {
                OrderId = "123",
                CustomerId = "ABC",
                ReceiveTimestamp = DateTime.UtcNow
            };

            var orderStock = new OrderStock
            {
                OrderId= orderSubmitted.OrderId
            };
            //await machine.TransitionToState(instance, machine.Initial);
            await machine.RaiseEvent(instance, machine.提交订单, orderSubmitted);
            await machine.RaiseEvent(instance, machine.扣除库存, orderStock);
            await machine.RaiseEvent(instance, machine.OrderCancellationRequested);
            //await machine.RaiseEvent(instance, machine.提交订单, orderSubmitted);
            //await machine.RaiseEvent(instance, machine.提交订单, orderSubmitted);
            //await machine.RaiseEvent(instance, machine.OrderCancellationRequested);
        }
    }

    class Relationship
    {
        public State CurrentState { get; set; }
        public string Name { get; set; }
    }

    class RelationshipStateMachine :
        AutomatonymousStateMachine<Relationship>
    {
        public RelationshipStateMachine()
        {
            Initially(
                When(Hello)
                    .TransitionTo(Friend),
                When(PissOff)
                    .TransitionTo(Enemy),
                When(Introduce)
                    .Then(ctx => ctx.Instance.Name = ctx.Data.Name)
                    .TransitionTo(Enemy)
            );
        }

        public State Friend { get; private set; }
        public State Enemy { get; private set; }

        public Event Hello { get; private set; }
        public Event PissOff { get; private set; }
        public Event<Person> Introduce { get; private set; }
    }

    class Person
    {
        public string Name { get; set; }
    }
}
