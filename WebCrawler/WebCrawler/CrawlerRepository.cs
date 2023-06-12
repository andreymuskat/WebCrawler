using HtmlAgilityPack;

namespace WebCrawler
{
    public class CrawlerRepository : IClawler
    {
        private string _url;

        public CrawlerRepository()
        {
            _url = "https://tesera.ru/location/russia/moscow/wants/";
        }

        public async Task<List<GameLinkEntity>> getAllLinkGamesAsync()
        {

            var httpCLient = new HttpClient();
            var html = await httpCLient.GetStringAsync(_url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var games = new List<GameLinkEntity>();


            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(a => a.GetAttributeValue("class", "").Equals("gameslinked")).ToList();

            try
            {
                foreach (var div in divs)
                {
                    var game = new GameLinkEntity()
                    {
                        Link = "https://tesera.ru" + div.Descendants("a").FirstOrDefault().ChildAttributes("href").FirstOrDefault().Value,
                    };
                    games.Add(game);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return games;
        }

        public async Task<List<GameNamesEntity>> getAllNameAsync()
        {

            var httpCLient = new HttpClient();
            var html = await httpCLient.GetStringAsync(_url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var games = new List<GameNamesEntity>();

            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(a => a.GetAttributeValue("class", "").Equals("gameslinked")).ToList();

            try
            {
                foreach (var div in divs)
                {
                    var game = new GameNamesEntity()
                    {
                        NameGame = div.SelectSingleNode("//h3/a").InnerText,
                        NameAuthor = div.Descendants("a").First(x => x.Attributes["class"] != null && x.Attributes["class"].Value == "spic").InnerText,
                    };
                    games.Add(game);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return games;
        }

        public async Task<GameDescriptionEntity> getDescriptionGame(string urlGame)
        {
            var httpCLient = new HttpClient();
            var html = await httpCLient.GetStringAsync(urlGame);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var div = htmlDocument.DocumentNode.Descendants("div")
                .Where(a => a.GetAttributeValue("class", "").Equals("leftcol")).ToList();

            var gameDescription = new GameDescriptionEntity();

            try
            {
                foreach (var obj in div)
                {
                    var descriptionElements = obj.SelectSingleNode("//div[@class='main']").ChildNodes;
                    string description = string.Join("", descriptionElements.Select(e => e.InnerText.Trim()));

                    description = description
                         .Replace("&mdash", "")
                         .Replace("&nbsp;", "");

                    gameDescription = new GameDescriptionEntity()
                    {
                        NameAuther = obj.Descendants("span")
                                        .FirstOrDefault(x => x.Attributes["itemprop"] != null && x.Attributes["itemprop"].Value == "author")?
                                        .InnerText,
                        Description = description,
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return gameDescription;
        }
    }
}
