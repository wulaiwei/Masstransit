using System;
using System.Threading.Tasks;
using MassTransit.Order.Contracts.Order;

namespace MassTransit.Order.Consumer.Order
{
    public class InsertOrderConsumer:IConsumer<IInsertOrder>
    {
        public async Task Consume(ConsumeContext<IInsertOrder> context)
        {
            await Console.Out.WriteLineAsync($"Updating customer: {context.Message.OrderId}");
        }
    }
}