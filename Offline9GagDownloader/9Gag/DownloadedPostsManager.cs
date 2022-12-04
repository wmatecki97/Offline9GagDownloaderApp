﻿using Offline9GagDownloader._9Gag.DB;

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

        public async Task<string> TryDownloadPostAsync(PostDefinition post, HttpClient client)
        {
            try
            {
                var posts = await postDatabase.GetItems();
                if (posts.Any(p => p.SrcUrl == post.ImgSrc))
                {
                    return null;
                }

                var media = await client.GetByteArrayAsync(post.ImgSrc);

                string storageFileName = await SaveMediaOnDisc(post, media);

                var postModel = new PostModel(post, storageFileName);
                await postDatabase.AddItem(postModel);

                return storageFileName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static async Task<string> SaveMediaOnDisc(PostDefinition post, byte[] media)
        {
            var storageFileName = GetStorageFileName(post.ImgSrc);

            if (!Directory.Exists(StorageDirectoryName))
            {
                Directory.CreateDirectory(StorageDirectoryName);
            }

            await File.WriteAllBytesAsync(storageFileName, media);
            return storageFileName;
        }

        private static string GetStorageFileName(string url)
        {
            var filename = Path.GetFileName(url);
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return $"{StorageDirectoryName}/{filename}";
        }
    }
}
