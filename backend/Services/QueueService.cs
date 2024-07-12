using System;
using RabbitMQ.Client;

namespace nginx_deployer.Services
{
    public class QueueService
    {
        // Add your class members and methods here
         private readonly IModel _channel;
        private readonly IConnection _connection;

        public QueueService()
        {
            ConnectionFactory factory = new()
            {
                HostName = "rabbitmq",
                UserName = "user",
                Password = "password",
                Port = 5672
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void AddToDeployQueue(string message)
        {
            try
            {
                _channel.QueueDeclare(queue: "deploy",
                                      durable: true,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                var body = System.Text.Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: "",
                                      routingKey: "deploy",
                                      basicProperties: null,
                                      body: body);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
    
        }
    }
}

