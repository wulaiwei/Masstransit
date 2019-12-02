using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Request.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MassTransit.Request.EventPublisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        IRequestClient<CheckOrderStatus> _client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRequestClient<CheckOrderStatus> client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("CheckOrderStatus")]
        public async Task<OrderStatusResult> CheckOrderStatus()
        {
            //var serviceAddress = new Uri("rabbitmq://localhost/orderStatus_check_queue");
            //var client = _bus.CreateRequestClient<CheckOrderStatus>(serviceAddress);

            var response = await _client.GetResponse<OrderStatusResult>(new { OrderId = "123" });

    

        

            return null;
        }
    }
}
