using SQLite;

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
            var result = await Database.CreateTableAsync<PostModel>();
        }

        public async Task<List<PostModel>> GetItems()
        {
            await Init();
            return await Database.Table<PostModel>().ToListAsync();
        }

        public async Task<int> AddItem(PostModel item)
        {
            await Init();
            return await Database.InsertAsync(item);
        }


        public async Task<int> DeleteItem(PostModel item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
