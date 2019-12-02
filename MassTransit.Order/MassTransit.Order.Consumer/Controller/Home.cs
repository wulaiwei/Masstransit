using Microsoft.AspNetCore.Mvc;

namespace MassTransit.Order.Consumer.Controller
{
    public class Home : Microsoft.AspNetCore.Mvc.Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok(new
            {
                success = true
            });
        }
    }
}