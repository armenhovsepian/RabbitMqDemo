using RabbitMQ.Client;
using RabbitMqDemo.Messaging;
using System;
using System.Text;

namespace RabbitMqDemo.Producer
{
    class RabbitMQManager : IDisposable
    {
        private readonly IModel _channel;
        public RabbitMQManager()
        {
            var connectionFactory = new ConnectionFactory() { Uri = new Uri(RabbitMqConstants.RabbitMqUri) };
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public void SendMessage(string message)
        {
            // Declaring RabbitMQ resources:
            // - Exchange
            // - Queue
            // - QueueBind
            _channel.ExchangeDeclare(
                exchange: RabbitMqConstants.DemoExchange,
                type: ExchangeType.Direct);

            _channel.QueueDeclare(
                queue: RabbitMqConstants.DemoQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(
                exchange: RabbitMqConstants.DemoExchange,
                queue: RabbitMqConstants.DemoQueue,
                routingKey: "");

            // if the message is object
            //var serializedMessage = JsonConvert.DeserializeObject(message);
            //var messageProperties = _channel.CreateBasicProperties();
            //messageProperties.ContentType = RabbitMqConstants.JsonMimeType;
            //var body = Encoding.UTF8.GetBytes(serializedMessage);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                                exchange: RabbitMqConstants.DemoExchange,
                                routingKey: "",
                                basicProperties: null, // messageProperties
                                body: body);
        }

        public void Dispose()
        {
            if (!_channel.IsClosed)
                _channel.Close();
        }
    }
}
