using Newtonsoft.Json;
using Offline9GagDownloader._9Gag;
using System.ComponentModel;

namespace Offline9GagDownloader;

public partial class MainPage : ContentPage
{
    private const int PagesToScrollWhenDownloadingCount = 10;
    private readonly IDownloadedPostsManager downloadedPostsManager;
    private readonly IHttpClientFactory httpClientFactory;
    int postsCount = 0;
    private bool _isDownloadInProgress = false;

    public MainPage(IDownloadedPostsManager downloadedPostsManager, IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();
        this.downloadedPostsManager = downloadedPostsManager;
        this.httpClientFactory = httpClientFactory;
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
        postsCount = await downloadedPostsManager.GetNotBrowsedMemesCount();
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
        Button button = sender as Button;
        button.IsVisible = false;

        _isDownloadInProgress = true;
        CancelDownloadBtn.IsVisible = true;

        ProgressBar progressBar = DownloadProgressBarr;
        var progresStepSizePerScroll = 1f / PagesToScrollWhenDownloadingCount;

		for(int i= 0; i < PagesToScrollWhenDownloadingCount; i++)
        {
            if(!_isDownloadInProgress) 
                break;

            await gagView.EvaluateJavaScriptAsync("window.scrollTo(0, document.body.scrollHeight)");
            await Task.Delay(200);//9gag crashes sometimes when scrolling too fast
            PostDefinition[] posts = await GetPostsFromWebView();

            var memesToDownload = await downloadedPostsManager.FilterOutAlreadySeenMemesAsync(posts);

            var downloadTasks = memesToDownload.Select(m => downloadedPostsManager.TryDownloadPostAsync(m, httpClientFactory.CreateClient()));

            await Task.WhenAll(downloadTasks);
            await progressBar.ProgressTo(progressBar.Progress + progresStepSizePerScroll, 1, Easing.Default);

            await UpdateUIData();
        }

        _isDownloadInProgress = false;
        CancelDownloadBtn.IsVisible = false;
        button.IsVisible = true;
        progressBar.Progress = 0;
    }

    private void OnCancelDownloadClick(object sender, EventArgs e)
    {
        _isDownloadInProgress = false;
    }

    private async Task<PostDefinition[]> GetPostsFromWebView()
    {
        var postsString = await gagView.EvaluateJavaScriptAsync(JsScripts.GetPosts);
        var postsMobileString = await gagView.EvaluateJavaScriptAsync(JsScripts.GetPostsMobile);
        postsString = postsString != "[]" ? postsString : postsMobileString;
        postsString = postsString.Replace("\\\"", "\"").Replace("\\\\", "\\");
        var posts = JsonConvert.DeserializeObject<PostDefinition[]>(postsString);
        return posts;
    }
}

