using System.Linq.Expressions;

namespace Offline9GagDownloader._9Gag.DB
{
    internal interface IPostDatabase
    {
        Task<int> SavePost(PostModel item);
        Task<int> DeleteItem(PostModel item);
        Task<bool> CehckIfMediaUrlExist(string url);
        Task<int> GetNotBrowsedMemesCount();
        Task<List<PostModel>> GetItems();
        Task<List<PostModel>> GetItems(Expression<Func<PostModel, bool>> filterExpression);
    }
}