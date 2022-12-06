using Offline9GagDownloader._9Gag.DB;

namespace Offline9GagDownloader._9Gag
{
    public interface IDownloadedPostsManager
    {
        Task<List<PostModel>> GetAllSavedPosts();
        Task<bool> TryDownloadPostAsync(PostDefinition postDefinition, HttpClient client);
        void DeletePost(PostModel currentPost);
    }
}