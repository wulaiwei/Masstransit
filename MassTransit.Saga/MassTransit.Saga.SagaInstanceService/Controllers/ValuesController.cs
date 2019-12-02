using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Saga.EntityFramework.Event;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.Saga.SagaInstanceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IBusControl _publishEndpoint;
        public ValuesController(IBusControl publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [Route("saga")]
        public void CreateSagaInstance()
        {
            var res= _publishEndpoint.Publish<InsertSagaInstance>(new
            {
                ServiceName="测试",
                CorrelationId = Guid.NewGuid()
            });
        }
        
        [HttpGet]
        [Route("sagaCancle")]
        public void CancleSagaInstance()
        {
            
            var res= _publishEndpoint.Publish<ICancelSagaInstance>(new
            {
                ServiceName="测试",
                CorrelationId=Guid.Parse("ad291f5b-b2be-4475-89e3-1be83f5593c2")
            });
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }


    }
}
