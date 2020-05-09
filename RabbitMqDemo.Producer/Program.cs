using System;
using System.Threading;

namespace RabbitMqDemo.Producer
{
    class Program
    {
        static void Main(string[] args)
        {

            var rabbitMqManager = new RabbitMQManager();

            while (true)
            {
                string message = $"Hello from Producer ({DateTime.Now})";
                rabbitMqManager.SendMessage(message);

                Console.WriteLine("Message sent");
                Thread.Sleep(1000);
            }
        }
    }
}
