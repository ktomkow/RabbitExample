using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Settings;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    public class ReceiverWorker
    {
        private readonly string queueName;
        private readonly string exchangeName;
        private readonly string routingKey;
        private readonly string brokerAddress;

        private readonly QueueCredentials credentials;

        public ReceiverWorker(QueueSettings queueSettings)
        {
            if (queueSettings is null)
            {
                throw new ArgumentNullException(nameof(queueSettings));
            }

            this.queueName = queueSettings.Queue;
            this.exchangeName = queueSettings.Exchange;
            this.routingKey = queueSettings.RoutingKey;
            this.brokerAddress = queueSettings.BrokerAddress;

            this.credentials = queueSettings.Credentials;
        }

        public void Work()
        {
            ConnectionFactory factory = new ConnectionFactory();

            factory.Uri = new Uri($"amqp://{this.credentials.User}:{this.credentials.Password}@{this.brokerAddress}");
            factory.DispatchConsumersAsync = true;

            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (ch, deliverEventArgs) =>
            {
                byte[] body = deliverEventArgs.Body.ToArray();
                string result = Encoding.UTF8.GetString(body);

                Console.WriteLine(result);

                channel.BasicAck(deliverEventArgs.DeliveryTag, false);
                await Task.Yield();
            };

            string consumerTag = channel.BasicConsume(queueName, false, consumer);

            Console.ReadLine();

            //channel.Close();
            //conn.Close();
        }
    }
}
