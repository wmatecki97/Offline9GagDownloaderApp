using Newtonsoft.Json;
using Offline9GagDownloader._9Gag;

namespace Offline9GagDownloader;

public partial class MainPage : ContentPage
{
    private readonly IDownloadedPostsManager downloadedPostsManager;
    int postsCount = 0;

	public MainPage(IDownloadedPostsManager downloadedPostsManager)
	{
		InitializeComponent();
        this.downloadedPostsManager = downloadedPostsManager;
        Task.Run(async () => await UpdateUIData());
    }

    private async Task UpdateUIData()
    {
        var posts = await downloadedPostsManager.GetAllSavedPosts();
        postsCount = posts.Where(p => !p.Displayed).Count();
        await Dispatcher.DispatchAsync(() => UpdateStatisticsLabel());
    }

    private void UpdateStatisticsLabel()
    {
        StatisticsLabel.Text = $"Saved posts:{postsCount}";
    }

    private async void OnBrowseClick(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new BrowsePage(downloadedPostsManager));
    }

    private async void OnDownloadClick(object sender, EventArgs e)
	{
        using var client = new HttpClient();
		for(int i= 0; i < 10; i++)
        {
            await gagView.EvaluateJavaScriptAsync("window.scrollTo(0, document.body.scrollHeight)");
            await Task.Delay(1000);
            PostDefinition[] posts = await GetPostsFromWebView();

            foreach (var post in posts)
            {
                var success = await downloadedPostsManager.TryDownloadPostAsync(post, client);
            }

            await UpdateUIData();
        }

        await Navigation.PushAsync(new BrowsePage(downloadedPostsManager));
	}

    private async Task<PostDefinition[]> GetPostsFromWebView()
    {
        var postsString = await gagView.EvaluateJavaScriptAsync(JsScripts.GetPosts);
        var postsMobileString = await gagView.EvaluateJavaScriptAsync(JsScripts.GetPostsMobile);
        postsString = postsString != "[]" ? postsString : postsMobileString;
        postsString = postsString.Replace("\\\"", "\"").Replace("\\\\", "\\");
        Console.WriteLine(postsString);
        var posts = JsonConvert.DeserializeObject<PostDefinition[]>(postsString);
        return posts;
    }
}

