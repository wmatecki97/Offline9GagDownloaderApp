using Offline9GagDownloader._9Gag.DB;

namespace Offline9GagDownloader._9Gag
{
    internal class DownloadedPostsManager : IDownloadedPostsManager
    {
        private readonly IPostDatabase postDatabase;
        private static string StorageDirectoryName = Path.Combine(FileSystem.Current.CacheDirectory, "nineGagStorage");
        public DownloadedPostsManager(IPostDatabase postDatabase)
        {
            this.postDatabase = postDatabase;
        }

        public async Task<List<PostModel>> GetAllSavedPosts()
        {
            var posts = await postDatabase.GetItems();
            return posts;
        }

        public async Task<bool> TryDownloadPostAsync(PostDefinition post, HttpClient client)
        {
            try
            {
                var posts = await postDatabase.GetItems();

                if (posts.Any(p => p.SrcUrl == post.ImgSrc))
                {
                    return false;
                }

                var media = await client.GetByteArrayAsync(post.ImgSrc);

                string storageFileName = await SaveMediaOnDisc(post, media);

                var postModel = new PostModel(post, storageFileName);
                await postDatabase.SavePost(postModel);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void DeletePost(PostModel currentPost)
        {
            if (currentPost is null)
                return;

            currentPost.Displayed = true;
            postDatabase.SavePost(currentPost);
            File.Delete(currentPost.MediaPath);
        }

        private static async Task<string> SaveMediaOnDisc(PostDefinition post, byte[] media)
        {
            var storageFileName = GetStorageFileName(post.ImgSrc);

            if (!Directory.Exists(StorageDirectoryName))
            {
                Directory.CreateDirectory(StorageDirectoryName);
            }

            await File.WriteAllBytesAsync(storageFileName, media);
            var fileCanBeRead = await File.ReadAllBytesAsync(storageFileName);
            return storageFileName;
        }

        private static string GetStorageFileName(string url)
        {
            var filename = Path.GetFileName(url);
            return Path.Combine(StorageDirectoryName,filename);
        }
    }
}
