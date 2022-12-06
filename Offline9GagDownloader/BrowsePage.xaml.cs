using Offline9GagDownloader._9Gag;
using Offline9GagDownloader._9Gag.DB;

namespace Offline9GagDownloader;

public partial class BrowsePage : ContentPage
{
    private const int StandardPostSize = 640;
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
        PostModel nextPost = await GetNextPost();
        if (nextPost is null)
            return;

        AdjustMediaWidth();

        UpdateMedia(nextPost);

        button.IsEnabled = true;
    }

    private void UpdateMedia(PostModel nextPost)
    {
        Title.Text = nextPost.Title;
        if (Path.GetExtension(nextPost.MediaPath) == ".jpg")
        {
            image.Source = nextPost.MediaPath;
            image.IsVisible = true;
            video.IsVisible = false;
            video.Pause();
        }
        else
        {
            video.Source = new VideoPlayback.Controls.FileVideoSource
            {
                File = nextPost.MediaPath
            };
            image.IsVisible = false;
            video.IsVisible = true;
        }
    }

    private void AdjustMediaWidth()
    {
        var postWidth = DeviceDisplay.MainDisplayInfo.Width < StandardPostSize ? DeviceDisplay.MainDisplayInfo.Width : StandardPostSize;
        video.WidthRequest = postWidth;
        image.WidthRequest = postWidth;
    }

    private async Task<PostModel> GetNextPost()
    {
        if (posts is null)
        {
            posts = await postsManager.GetAllSavedPosts();
        }
        if(index == posts.Count)
        {
            Shell.Current.SendBackButtonPressed();
            return null;
        }
        var nextPost = posts[index++];
        return nextPost;
    }

    void OnContentPageUnloaded(object sender, EventArgs e)
    {
        video.Handler?.DisconnectHandler();
    }
}