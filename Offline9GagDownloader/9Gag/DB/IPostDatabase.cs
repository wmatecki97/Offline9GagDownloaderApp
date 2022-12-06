namespace Offline9GagDownloader._9Gag.DB
{
    internal interface IPostDatabase
    {
        Task<int> SavePost(PostModel item);
        Task<int> DeleteItem(PostModel item);
        Task<List<PostModel>> GetItems();
    }
}