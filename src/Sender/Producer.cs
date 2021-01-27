using RabbitMQ.Client;
using Settings;
using System;

namespace Sender
{
    public class Producer
    {
        private readonly string queueName;
        private readonly string exchangeName;
        private readonly string routingKey;
        private readonly string brokerAddress;

        private readonly QueueCredentials credentials;

        public Producer(QueueSettings queueSettings)
        {
            if(queueSettings is null)
            {
                throw new ArgumentNullException(nameof(queueSettings));
            }

            this.queueName = queueSettings.Queue;
            this.exchangeName = queueSettings.Exchange;
            this.routingKey = queueSettings.RoutingKey;
            this.brokerAddress = queueSettings.BrokerAddress;

            this.credentials = queueSettings.Credentials;
        }

        public void Send(string text, string sender)
        {
            ConnectionFactory factory = new ConnectionFactory();

            factory.Uri = new Uri($"amqp://{this.credentials.User}:{this.credentials.Password}@{this.brokerAddress}");

            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare(this.exchangeName, ExchangeType.Direct);

            bool isQueueDurable = false;
            channel.QueueDeclare(this.queueName, isQueueDurable, false, false, null);
            channel.QueueBind(this.queueName, this.exchangeName, this.routingKey, null);

            var payload = this.Convert($"{sender} says {text}");

            IBasicProperties props = channel.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.DeliveryMode = 2;
            channel.BasicPublish(this.exchangeName, this.routingKey, props, payload);

            channel.Close();
            conn.Close();
        }

        private byte[] Convert(string text)
        {
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return messageBodyBytes;
        }
    }
}
