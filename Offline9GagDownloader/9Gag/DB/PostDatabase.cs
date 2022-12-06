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

        public async Task<int> SavePost(PostModel post)
        {
            await Init();

            if (post.Id != 0)
            {
                // Update an existing note.
                return await Database.UpdateAsync(post);
            }
            else
            {
                // Save a new note.
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
