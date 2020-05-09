using System;

namespace RabbitMqDemo.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var rabbitMqManager = new RabbitMQManager();
            rabbitMqManager.ListenForMessage();

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
