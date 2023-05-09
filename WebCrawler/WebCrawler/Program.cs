using System.Net.Http;
using HtmlAgilityPack;

namespace WebCrawler
{
    class Program
    {
        static void Main()
        {
            startCrawlerAsync();
            Console.ReadLine();
        }

        private static async Task startCrawlerAsync()
        {
            var url = "https://tesera.ru/location/russia/moscow/wants/";
            var httpCLient = new HttpClient();
            var html = await httpCLient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var games = new List<Game>();


            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(a => a.GetAttributeValue("class", "").Equals("gameslinked")).ToList();

            try
            {
                foreach (var div in divs)
                {
                    var game = new Game()
                    {
                        NameGame = div.SelectSingleNode("//h3/a").InnerText,
                        NameAuthor = div.Descendants("a").First(x => x.Attributes["class"] != null && x.Attributes["class"].Value == "spic").InnerText,
                        Rating = div.Descendants("span").FirstOrDefault().InnerText,
                        ImageUrl = div.Descendants("img").FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value,
                        Link = div.Descendants("a").FirstOrDefault().ChildAttributes("href").FirstOrDefault().Value,
                    };
                    games.Add(game);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}