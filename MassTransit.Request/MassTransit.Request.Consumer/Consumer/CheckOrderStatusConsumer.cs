using MassTransit.Request.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Request.Consumer.Consumer
{
    public class CheckOrderStatusConsumer :
     IConsumer<CheckOrderStatus>
    {
        public async Task Consume(ConsumeContext<CheckOrderStatus> context)
        {
            await context.RespondAsync<OrderStatusResult>(new
            {
                OrderId = Guid.NewGuid().ToString(),
                Timestamp=DateTime.Now,
                StatusCode= "StatusCode",
                StatusText= "StatusText"
            });
        }
    }
}
