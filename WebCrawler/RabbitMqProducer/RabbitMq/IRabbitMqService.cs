namespace RabbitMqProducer.RabbitMq
{
    public interface IRabbitMqService
    {
        Task SendMessageAsync();
    }
}
