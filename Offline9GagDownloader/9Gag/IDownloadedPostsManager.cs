using Offline9GagDownloader._9Gag.DB;

namespace Offline9GagDownloader._9Gag
{
    public interface IDownloadedPostsManager
    {
        Task<List<PostModel>> GetAllSavedPosts();
        Task<string> TryDownloadPostAsync(PostDefinition postDefinition, HttpClient client);
    }
}