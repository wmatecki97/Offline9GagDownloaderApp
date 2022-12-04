using Offline9GagDownloader._9Gag.DB;

namespace Offline9GagDownloader._9Gag
{
    internal class DownloadedPostsManager : IDownloadedPostsManager
    {
        private readonly IPostDatabase postDatabase;
        private static string StorageDirectoryName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/NineGagDownloader";
        public DownloadedPostsManager(IPostDatabase postDatabase)
        {
            this.postDatabase = postDatabase;
        }

        public async Task<string> TryDownloadPostAsync(PostDefinition post)
        {
            try
            {
                using var client = new HttpClient();
                var media = await client.GetByteArrayAsync(post.ImgSrc);

                var storageFileName = GetStorageFileName(post.ImgSrc);
               
                if (!Directory.Exists(StorageDirectoryName))
                {
                    Directory.CreateDirectory(StorageDirectoryName);
                }

                await File.WriteAllBytesAsync(storageFileName, media);

                var postModel = new PostModel(post, storageFileName);
                await postDatabase.AddItem(postModel);

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
