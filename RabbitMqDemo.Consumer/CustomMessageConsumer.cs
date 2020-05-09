using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMqDemo.Messaging;
using System;
using System.Text;

namespace RabbitMqDemo.Consumer
{
    class CustomMessageConsumer : DefaultBasicConsumer
    {
        private readonly IModel _channel;
        public CustomMessageConsumer(IModel channel) => _channel = channel;

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            if (properties.ContentType != RabbitMqConstants.JsonMimeType)
                throw new ArgumentException($"Can't handle conntent type {properties.ContentType}");

            var message = Encoding.UTF8.GetString(body.ToArray());
            var messageObject = JsonConvert.DeserializeObject(message);

            Consume(messageObject);

            SendAck(deliveryTag);
        }

        private void Consume(object messageObject)
        {
            // TODO: do some action(s) like 
            // - store to persistence
            // - notify subscribers and reply

            _channel.ExchangeDeclare(exchange: RabbitMqConstants.DemoNotifyExchange,
                type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: RabbitMqConstants.DemoNotifyQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueBind(exchange: RabbitMqConstants.DemoNotifyExchange,
                queue: RabbitMqConstants.DemoNotifyQueue,
                routingKey: "");

            var serializedMessage = JsonConvert.SerializeObject(messageObject);
            var messageProperties = _channel.CreateBasicProperties();
            messageProperties.ContentType = RabbitMqConstants.JsonMimeType;
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            _channel.BasicPublish(
                                exchange: RabbitMqConstants.DemoNotifyExchange,
                                routingKey: "",
                                basicProperties: messageProperties,
                                body: body);

            Console.WriteLine("Message Received: {0}", (string)messageObject);
        }

        void SendAck(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }
    }
}
