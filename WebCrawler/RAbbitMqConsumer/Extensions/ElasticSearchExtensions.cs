using Nest;
using WebCrawler;

namespace RAbbitMqConsumer.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                                                    .DefaultIndex("elk_index");

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, "elk_index");
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<GameDescriptionEntity>(x => x.AutoMap())
            );
        }
    }
}
