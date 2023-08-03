using SearchSyncService.Services;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SearchSyncService.Consumers.Models;

namespace SearchSyncService.Consumers;

public class StudentEventEsSyncConsumer
{
    private readonly StudentSync _studentSync;
    
    public StudentEventEsSyncConsumer(StudentSync studentSync)
    {
        _studentSync = studentSync;
    }

    public async Task Consume()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672"),
            ClientProvidedName = "RabbitMQ Consumer"
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var exchangeName = "DemoExchange";
            var routingKey = "demo-routing-key";
            var queueName = "DemoQueue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queueName, exchangeName, routingKey, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");
                var changeEvent = JsonSerializer.Deserialize<BaseModel>(message);
                // Process the message here
                // ...
                _studentSync.ProcessChangeEventAsync(changeEvent);
                channel.BasicAck(args.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queueName, autoAck: false, consumer: consumer);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
    
}