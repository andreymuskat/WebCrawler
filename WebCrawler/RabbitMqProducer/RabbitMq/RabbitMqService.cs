using RabbitMQ.Client;
using RabbitMqProducer.RabbitMq;
using System.Text;
using WebCrawler;
using Newtonsoft.Json;

public class RabbitMqService : IRabbitMqService
{
    public void SendMessage()
    {
        SendMessageAsync();
    }

    public async Task SendMessageAsync()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                // Создание очереди
                channel.QueueDeclare(queue: "game_tasks", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var crawler = new CrawlerRepository();

                var games = await crawler.getAllLinkGamesAsync();

                // Отправка задач в очередь
                foreach (var game in games)
                {
                    var serializedGame = JsonConvert.SerializeObject(game);
                    var body = Encoding.UTF8.GetBytes(serializedGame);
                    channel.BasicPublish(exchange: "", routingKey: "game_tasks", basicProperties: null, body: body);
                }
            }
        }
    }
}