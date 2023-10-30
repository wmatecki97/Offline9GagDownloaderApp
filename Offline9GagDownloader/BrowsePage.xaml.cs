using Offline9GagDownloader._9Gag;
using Offline9GagDownloader._9Gag.DB;
using VideoPlayback.Controls;

namespace Offline9GagDownloader;

public partial class BrowsePage : ContentPage
{
    private const int StandardPostSize = 640;
    private readonly IDownloadedPostsManager postsManager;
    private List<PostModel> posts;
    private int postsCount;
    PostModel currentPost;
    private int index = 0;

    public BrowsePage(IDownloadedPostsManager postsManager)
    {
        InitializeComponent();
        this.postsManager = postsManager;
        Task.Run(async () => await Init());
    }

    private async Task Init()
    {
        var allPosts = await postsManager.GetAllSavedPosts();
        posts = allPosts.Where(p => !p.Displayed).ToList();
        postsCount = posts.Count();
        await Dispatcher.DispatchAsync(() => UpdateStatisticsLabel());
        await Dispatcher.DispatchAsync(() => LoadFirstPost());
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Video.Stop();
    }

    private void UpdateStatisticsLabel()
    {
        StatisticsLabel.Text = $"Saved posts:{postsCount}";
    }

    void LoadFirstPost()
    {
        currentPost = GetNextPost();
        if (currentPost is null)
            return;

        AdjustMediaWidth();
        UpdateMedia(currentPost);
    }

    void NextPostButtonClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        button.IsEnabled = false;

        //remove old from memory
        CreateNewMediaViews();
        postsManager.DeletePost(currentPost);
        postsCount--;
        UpdateStatisticsLabel();

        //load new post
        currentPost = GetNextPost();
        if (currentPost is null)
            return;

        AdjustMediaWidth();
        UpdateMedia(currentPost);

        button.IsEnabled = true;
    }

    private void CreateNewMediaViews()
    {
        MediaStackLayout.Children.Clear();
        Video = new Video
        {
            HorizontalOptions = LayoutOptions.Center,
        };
        Image = new Image
        {
            HorizontalOptions = LayoutOptions.Center
        };
        MediaScrollView = new ScrollView()
        {
            Content = Image
        };
        MediaStackLayout.Children.Add(Video);
        MediaStackLayout.Children.Add(MediaScrollView);

        MediaStackLayout.Children.Add()
        //to prevent unused video files of using the memory
        GC.Collect();
    }

    private void UpdateMedia(PostModel nextPost)
    {
        Title.Text = nextPost.Title;
        if (Path.GetExtension(nextPost.MediaPath) == ".jpg")
        {
            Image.Source = nextPost.MediaPath;
            Image.IsVisible = true;
            Video.IsVisible = false;
            Video.Pause();
        }
        else
        {
            Console.WriteLine(nextPost.MediaPath);
            Video.Source = new FileVideoSource
            {
                File = nextPost.MediaPath
            };
            Image.IsVisible = false;
            Video.IsVisible = true;
        }
        foreach (var tag in nextPost.Tags)
        {
            MediaStackLayout.Children.Add(new Label(){Text=tag});
        }
    }

    private void AdjustMediaWidth()
    {
        var postWidth = DeviceDisplay.MainDisplayInfo.Width < StandardPostSize ? DeviceDisplay.MainDisplayInfo.Width : StandardPostSize;
        postWidth/=1.8;//ugly hack since phone width is not woking as expected
        Video.WidthRequest = postWidth;
        Image.WidthRequest = postWidth;
    }

    private PostModel GetNextPost()
    {
        if(index == posts.Count)
        {
            Shell.Current.SendBackButtonPressed();
            return null;
        }

        var nextPost = posts[index++];
        return nextPost;
    }
}