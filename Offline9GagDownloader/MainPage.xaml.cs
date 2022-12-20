using Newtonsoft.Json;
using Offline9GagDownloader._9Gag;
using System.ComponentModel;

namespace Offline9GagDownloader;

public partial class MainPage : ContentPage
{
    private readonly IDownloadedPostsManager downloadedPostsManager;
    int postsCount = 0;

	public MainPage(IDownloadedPostsManager downloadedPostsManager)
	{
		InitializeComponent();
        this.downloadedPostsManager = downloadedPostsManager;
        CategoryPicker.SelectedIndex = 0;
        CategoryPicker.SelectedIndexChanged += CategoryChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(async () => await UpdateUIData());
    }

    private void CategoryChanged(object sender, EventArgs e)
    {
        if(CategoryPicker.SelectedIndex > 0)
        {
            gagView.Source = $"https://www.9gag.com/{CategoryPicker.SelectedItem}";
        }
    }

    private async Task UpdateUIData()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await Navigation.PushAsync(new BrowsePage(downloadedPostsManager));
        }
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
            await Task.Delay(200);//9gag crashes sometimes when scrolling too fast
            PostDefinition[] posts = await GetPostsFromWebView();

            foreach (var post in posts)
            {
                await downloadedPostsManager.TryDownloadPostAsync(post, client);
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

