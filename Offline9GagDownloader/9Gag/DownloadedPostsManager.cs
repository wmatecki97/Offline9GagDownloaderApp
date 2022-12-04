using Offline9GagDownloader._9Gag.DB;

namespace Offline9GagDownloader._9Gag
{
    internal class DownloadedPostsManager : IDownloadedPostsManager
    {
        private readonly IHttpClientFactory factory;
        private readonly PostsDbContext dbContext;
        private static string StorageDirectoryName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/NineGagDownloader";
        public DownloadedPostsManager(IHttpClientFactory factory, PostsDbContext dbContext)
        {
            this.factory = factory;
            this.dbContext = dbContext;
        }

        public async Task<string> TryDownloadPostAsync(PostDefinition post)
        {
            try
            {
                using var client = factory.CreateClient();
                var media = await client.GetByteArrayAsync(post.ImgSrc);

                var storageFileName = GetStorageFileName(post.ImgSrc);
               
                if (!Directory.Exists(StorageDirectoryName))
                {
                    Directory.CreateDirectory(StorageDirectoryName);
                }

                await File.WriteAllBytesAsync(storageFileName, media);

                var postModel = new PostModel(post, storageFileName);
                dbContext.Posts.Add(postModel);
                await dbContext.SaveChangesAsync();

                return storageFileName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string GetStorageFileName(string url)
        {
            var filename = Path.GetFileName(url);
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return $"{StorageDirectoryName}/{filename}";
        }
    }
}
