using Settings;
using System;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            string senderName;

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

            var sender = new Producer(settings);

            string input;

            Console.Write("Please introduce yourself: ");
            senderName = Console.ReadLine();
            Console.WriteLine($"Hello {senderName}, write a message to send it to the queue");
            Console.WriteLine("Type q to quit");

            while(true)
            {
                Console.Write("Write a message: ");
                input = Console.ReadLine();
                if(input == "q")
                {
                    break;
                }

                sender.Send(input, senderName);
            } 

            Console.WriteLine("Ok, I'm done.");
        }
    }
}
