namespace Settings
{
    public class QueueSettings
    {
        public string Queue { get; set; }

        public string Exchange { get; set; }

        public string RoutingKey { get; set; }

        public string  BrokerAddress { get; set; }

        public QueueCredentials Credentials { get; set; }
    }
}
