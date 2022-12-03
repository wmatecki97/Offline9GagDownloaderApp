namespace Offline9GagDownloader._9Gag
{
    internal class DownloadedPostsManager : IDownloadedPostsManager
    {
        private readonly IHttpClientFactory factory;
        private static string StorageDirectoryName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/NineGagDownloader";
        public DownloadedPostsManager(IHttpClientFactory factory)
        {
            this.factory = factory;
        }

        public async Task<string> TryDownloadPostAsync(string title, string url)
        {
            try
            {
                using var client = factory.CreateClient();
                var media = await client.GetByteArrayAsync(url);

                var storageFileName = GetStorageFileName(url);
               
                if (!Directory.Exists(StorageDirectoryName))
                {
                    Directory.CreateDirectory(StorageDirectoryName);
                }

                await File.WriteAllBytesAsync(storageFileName, media);
                var f = await File.ReadAllBytesAsync(storageFileName);
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
