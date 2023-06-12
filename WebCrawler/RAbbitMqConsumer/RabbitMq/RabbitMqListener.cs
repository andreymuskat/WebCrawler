using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using WebCrawler;

public class RabbitMqListener : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqListener()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel.QueueDeclare(queue: "game_tasks", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var gameLink = JsonConvert.DeserializeObject<GameLinkEntity>(message);

            // Обработка полученной задачи
            var crawler = new CrawlerRepository();

            var games = await crawler.getDescriptionGame(gameLink.Link);

            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: "game_tasks", autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}