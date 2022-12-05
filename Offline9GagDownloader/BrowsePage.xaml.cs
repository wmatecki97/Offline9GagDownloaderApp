using Offline9GagDownloader._9Gag;
using Offline9GagDownloader._9Gag.DB;

namespace Offline9GagDownloader;

public partial class BrowsePage : ContentPage
{
    private readonly IDownloadedPostsManager postsManager;
    private List<PostModel> posts;
    private int index = 0;

    public BrowsePage(IDownloadedPostsManager postsManager)
    {
        InitializeComponent();
        this.postsManager = postsManager;
    }



    async void NextPostButtonClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        button.IsEnabled = false;

        //remove old from memory

        //load new title and video
        if (posts is null)
        {
            posts = await postsManager.GetAllSavedPosts();
        }
        var nextPost = posts[index++];

        video.Source = new VideoPlayback.Controls.FileVideoSource
        {
            File = nextPost.MediaPath
        };

        button.IsEnabled = true;
    }

    void OnContentPageUnloaded(object sender, EventArgs e)
    {
        video.Handler?.DisconnectHandler();
    }
}