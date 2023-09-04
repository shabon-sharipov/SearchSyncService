using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SearchSyncService.Consumers;

public class StudentEventEsSyncConsumer : IStudentEventEsSyncConsumer
{
    private readonly IStudentSyncService _studentSync;
    private readonly RabbitMQOptions _options;

    public StudentEventEsSyncConsumer(IStudentSyncService studentSync, IConfiguration configuration)
    {
        _studentSync = studentSync;
        _options = configuration.GetSection("RabbitMQ")?.Get<RabbitMQOptions>() ?? throw new ArgumentNullException("RabbitMQ");
    }

    public async Task Consume()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_options.Url),
            ClientProvidedName = _options.ClientProvidedName
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
            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");
                var changeEvent = JsonSerializer.Deserialize<BaseModel>(message);
                // Process the message here
                // ...
                await _studentSync.ProcessChangeEventAsync(changeEvent);
                channel.BasicAck(args.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queueName, autoAck: false, consumer: consumer);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        await Task.CompletedTask;
    }

}