using SQLite;
using System.Linq.Expressions;

namespace Offline9GagDownloader._9Gag.DB
{
    internal class PostDatabase : IPostDatabase
    {
        SQLiteAsyncConnection Database;

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<PostModel>();
        }

        public async Task<List<PostModel>> GetItems()
        {
            await Init();
            return await Database.Table<PostModel>().ToListAsync();
        }

        public async Task<int> GetNotBrowsedMemesCount()
        {
            await Init();
            return await Database.Table<PostModel>().Where(p => !p.Displayed).CountAsync();
        }

        public async Task<List<PostModel>> GetItems(Expression<Func<PostModel, bool>> filterExpression)
        {
            await Init();
            return await Database.Table<PostModel>().Where(filterExpression).ToListAsync();
        }

        public async Task<bool> CehckIfMediaUrlExist(string url)
        {
            await Init();
            var media = await Database.Table<PostModel>().FirstOrDefaultAsync(x => x.SrcUrl == url);
            return media != null;
        }

        public async Task<int> SavePost(PostModel post)
        {
            await Init();

            if (post.Id != 0)
            {
                return await Database.UpdateAsync(post);
            }
            else
            {
                return await Database.InsertAsync(post);
            }
        }


        public async Task<int> DeleteItem(PostModel item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
