using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMqDemo.Producer
{
    public class RabbitMQManager : IDisposable
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

        public void SendMessage(string message)
        {
            // Declaring RabbitMQ resources:
            // - Exchange
            // - Queue
            // - QueueBind
            _channel.ExchangeDeclare(
                exchange: _configuration["RabbitMqExchange"],
                type: ExchangeType.Direct);

            _channel.QueueDeclare(
                queue: _configuration["RabbitMqQueue"],
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(
                exchange: _configuration["RabbitMqExchange"],
                queue: _configuration["RabbitMqQueue"],
                routingKey: "");

            // if the message is object
            //var serializedMessage = JsonConvert.DeserializeObject(message);
            //var messageProperties = _channel.CreateBasicProperties();
            //messageProperties.ContentType = RabbitMqConstants.JsonMimeType;
            //var body = Encoding.UTF8.GetBytes(serializedMessage);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                                exchange: _configuration["RabbitMqExchange"],
                                routingKey: "",
                                basicProperties: null, // messageProperties
                                body: body);
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
