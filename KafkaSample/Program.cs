using RdKafka;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KafkaSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Producer 接受一个或多个 BrokerList
            using (Producer producer = new Producer("10.0.60.50:9092"))
            //发送到一个名为 testtopic 的Topic，如果没有就会创建一个
            using (Topic topic = producer.Topic("jwell-opt-log"))
            {
                var input = new BussinessDataOperationLog
                {
                    AppId = "jwell-cs",
                    AppName = "积微测试",
                    Function = "cs",
                    OptUser = "ce",
                    AfterModifyData = "1",
                    BeforeModifyData="1111111111111111111111",
                    OptTime=DateTime.Now.ToLongDateString()
                };
                //将message转为一个 byte[]
                byte[] data = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(input));
                DeliveryReport deliveryReport = await topic.Produce(data);

                Console.WriteLine($"发送到分区：{deliveryReport.Partition}, Offset 为: {deliveryReport.Offset}");
            
            }
            Console.WriteLine("Hello World!");
        }
    }
}
