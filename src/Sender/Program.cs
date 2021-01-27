using System;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
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

            var sender = new Producer(settings);

            string input;

            Console.WriteLine("Hello, write a message to send it to the queue");
            Console.WriteLine("Type q to quit");

            while(true)
            {
                Console.Write("Write a message: ");
                input = Console.ReadLine();
                if(input == "q")
                {
                    break;
                }

                sender.Send(input);
            } 

            Console.WriteLine("Ok, I'm done.");
        }
    }
}
