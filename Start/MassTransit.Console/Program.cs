using System;
using System.Threading.Tasks;

namespace MassTransit.Console
{
    public class ValueEntered {
        public string Value { get; set; }
    }

    public class ValueEntered1
    {
        public string Value { get; set; }
    }


    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg => cfg.Host(new Uri("rabbitmq://localhost"), action =>
            {
                action.Username("workdata");
                action.Password("workdata123!@#");
            }
            ));

            // Important! The bus must be started before using it!
            await busControl.StartAsync();
            try
            {
                do
                {
                    string value = await Task.Run(() =>
                    {
                        System.Console.WriteLine("Enter message (or quit to exit)");
                        System.Console.Write("> ");
                        return System.Console.ReadLine();
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    await busControl.Send<ValueEntered1>(new
                    {
                        Value = value
                    });
                }
                while (true);
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
