using MassTransit.RabbitMqTransport;
using System;
using System.Threading.Tasks;

namespace MassTransit.Start
{
    public class Message
    {
        public string Text { get; set; }
    }

    class Program
    {
        public static async Task Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost"),action=> {
                    action.Username("workdata");
                    action.Password("workdata123!@#");
                });

                sbc.ReceiveEndpoint("test_queue", ep =>
                {
                    ep.Handler<Message>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
                    });
                  
                });

                sbc.ReceiveEndpoint("test_queue1", ep =>
                {
                    ep.Handler<Message>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received1: {context.Message.Text}");
                    });
                });

                sbc.ReceiveEndpoint("MassTransit.Start.Message", ep =>
                {
                    ep.Handler<Message>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received2: {context.Message.Text}");
                    });
                });
            });

            await bus.StartAsync(); // This is important!
           
            await bus.Publish(new Message { Text = "Hi" });

            Console.WriteLine("Press any key to exit");
            await Task.Run(() => Console.ReadKey());

            await bus.StopAsync();
        }
    }
}
