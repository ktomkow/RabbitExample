using Settings;
using System;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {
        static void Main()
        {
            QueueSettings settings = new QueueSettings()
            {
                Queue = "LittleRabbit",
                Exchange = "MyLittleExchange",
                RoutingKey = "TheRoutingKey",
                BrokerAddress = "192.168.0.133:5672",
                Credentials = new QueueCredentials()
                {
                    User = "guest",
                    Password = "guest"
                }
            };

            Console.WriteLine("Started Receiver");

            ReceiverWorker receiver = new ReceiverWorker(settings);

            receiver.Work();

            Console.ReadLine();
        }
    }
}
