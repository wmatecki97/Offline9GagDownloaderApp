namespace Offline9GagDownloader._9Gag
{
    public interface IDownloadedPostsManager
    {
        Task<string> TryDownloadPostAsync(string title, string url);
    }
}