using System;
using System.Threading.Tasks;
using MassTransit.Order.Contracts.Order;
using MassTransit.Saga.EntityFramework.Event;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.Order.EventPublisher.Controller
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IBus _bus;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public HomeController(IBus bus, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _bus = bus;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        // GET
        public IActionResult Index()
        {
            return Ok(new
            {
                success = true
            });
        }

        public async Task<double> LoadCustomer(Guid orderId)
        {
            // work happens up in here
            return 0;
        }
        
        public async Task<IActionResult> PublisherMessage()
        {

            //            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/order_insert_queue1"));
            //            await endpoint.Send<IInsertOrder>(new
            //            {
            //                __Header_X_B3_TraceId = "123",
            //                __Header_X_B3_SpanId = "123",
            //                OrderId = Guid.NewGuid(),
            //                TotalPrice = 101,
            //                CreateTime = DateTime.Now
            //            });

            var res =  _publishEndpoint.Publish<InsertSagaInstance>(new
            {
                ServiceName = "测试",
                CorrelationId = Guid.NewGuid()
            });
            //await _publishEndpoint.Publish<IInsertOrder>(new
            //{
            //    __Header_X_B3_TraceId = "456",
            //    __Header_X_B3_SpanId = "456",
            //    OrderId = Guid.NewGuid(),
            //    TotalPrice = 100,
            //    CreateTime = DateTime.Now
            //});
            return Ok(new
            {
                success = true
            });
        }
    }
}