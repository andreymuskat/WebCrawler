namespace WebCrawler
{
    public interface IClawler
    {
       Task<List<GameLinkEntity>> getAllLinkGamesAsync();
    }
}
