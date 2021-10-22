using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqDemo.Consumer
{
    class RabbitMQManager : IDisposable
    {
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        public RabbitMQManager(IConfiguration configuration)
        {
            _configuration = configuration;
            // option 1
            //var connectionFactory = new ConnectionFactory() { Uri = new Uri(RabbitMqConstants.RabbitMqUri) };

            // option 2
            var connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = int.Parse(_configuration["RabbitMqPort"]),
                //UserName = "guest",
                //Password = "guest",
                //RequestedConnectionTimeout = TimeSpan.FromMilliseconds(3000) // milliseconds
            };

            try
            {
                var connection = connectionFactory.CreateConnection();
                _channel = connection.CreateModel();

                connection.ConnectionShutdown += ConnectionShutDown;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ==> host: {_configuration["RabbitMqHost"]}, port: {_configuration["RabbitMqPort"]}");
                Console.WriteLine($" ==> Can not connect to MessageBus: {ex.Message}");
            }
        }

        public void ListenForMessage()
        {
            _channel.QueueDeclare(queue: _configuration["RabbitMqQueue"],
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(prefetchSize: 0,
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
                Console.WriteLine(" ==> Message Received: {0}", message);
            };

            _channel.BasicConsume(queue: _configuration["RabbitMqQueue"],
                                 autoAck: true,
                                 consumer: consumer // customConsumer
                                 );
        }

        private void ConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine(" ==> RabbitMq connection shutdown");
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
                _channel.Close();
        }
    }
}
