namespace RabbitMqDemo.Messaging
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "amqp://guest:guest@localhost:5672/";
        public const string JsonMimeType = "application/json";

        public const string DemoExchange = "rabbitmqdemo.exchange";
        public const string DemoQueue = "rabbitmqdemo.message.queue";

        public const string DemoNotifyExchange = "rabbitmqdemo.notify.exchange";
        public const string DemoNotifyQueue = "rabbitmqdemo.notify.queue";

    }
}
