using Microsoft.AspNetCore.Mvc;
using Nest;
using WebCrawler;

namespace RAbbitMqConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ELKController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public ELKController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpGet(Name = "GetAllGames")]
        public async Task<IActionResult> Get(string keyword)
        {
            var result = await _elasticClient.SearchAsync<GameDescriptionEntity>(
                             s => s.Query(
                                 q => q.QueryString(
                                     d => d.Query('*' + keyword + '*')
                                 )).Size(5000));

            return Ok(result.Documents.ToList());
        }

        //[HttpGet(Name = "GetDuplicate")]
        //public async Task<IActionResult> GetDuplicate(string keyword)
        //{
        //    var result = await _elasticClient.SearchAsync<GameDescriptionEntity>(
        //        s => s.Query(q => q
        //            .Bool(b => b
        //                .Filter(f => f
        //                    .Regexp(r => r
        //                        .Field(fd => fd.Description) /
        //                        .Value(".*" + keyword + ".*") 
        //                    )
        //                )
        //            )
        //        )
        //        .Size(5000));

        //    return Ok(result.Documents.ToList());
        //}

        [HttpPost(Name = "AddGame")]
        public async Task<IActionResult> Post(GameDescriptionEntity game)
        {
            await _elasticClient.IndexDocumentAsync(game);

            return Ok();
        }
    }
}
