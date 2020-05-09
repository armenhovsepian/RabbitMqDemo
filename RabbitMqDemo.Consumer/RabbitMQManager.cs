using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqDemo.Messaging;
using System;
using System.Text;

namespace RabbitMqDemo.Consumer
{
    class RabbitMQManager : IDisposable
    {
        private readonly IModel _channel;
        public RabbitMQManager()
        {
            //var connectionFactory = new ConnectionFactory()
            //{
            //    HostName = "localhost",
            //    UserName = "guest",
            //    Password = "guest",
            //    Port = 5672,
            //    RequestedConnectionTimeout = TimeSpan.FromMilliseconds(3000), // milliseconds
            //};

            var connectionFactory = new ConnectionFactory() { Uri = new Uri(RabbitMqConstants.RabbitMqUri) };
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public void ListenForMessage()
        {
            _channel.QueueDeclare(queue: RabbitMqConstants.DemoQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(prefetchSize:0,
                prefetchCount: 1,
                global: false);

            var consumer = new EventingBasicConsumer(_channel);

            // we can define our custom consumer
            //var customConsumer = new CustomMessageConsumer(_channel);

            consumer.Received += (model, eventArgs) =>
            {
                //var contentType = eventArgs.BasicProperties.ContentType;
                //if (contentType != RabbitMqConstants.JsonMimeType)
                //    throw new ArgumentException($"Can't handle conntent type {contentType}");

                var body = eventArgs.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine("Message Received: {0}", message);
            };

            _channel.BasicConsume(queue: RabbitMqConstants.DemoQueue,
                                 autoAck: true,
                                 consumer: consumer // customConsumer
                                 );
        }

        public void Dispose()
        {
            if (!_channel.IsClosed)
                _channel.Close();
        }
    }
}
