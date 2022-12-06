using Newtonsoft.Json;
using Offline9GagDownloader._9Gag;

namespace Offline9GagDownloader;

public partial class MainPage : ContentPage
{
    private readonly IDownloadedPostsManager downloadedPostsManager;
    int count = 0;

	public MainPage(IDownloadedPostsManager downloadedPostsManager)
	{
		InitializeComponent();
        this.downloadedPostsManager = downloadedPostsManager;
    }



    private async void OnDownloadClick(object sender, EventArgs e)
	{
        using var client = new HttpClient();
		for(int i= 0; i < 1; i++)
		{
            await gagView.EvaluateJavaScriptAsync("window.scrollTo(0, document.body.scrollHeight)");

            await Task.Delay(1000);

            var postsString = await gagView.EvaluateJavaScriptAsync(JsScripts.GetPosts);
            var postsMobileString = await gagView.EvaluateJavaScriptAsync(JsScripts.GetPostsMobile);
			postsString = postsString != "[]" ? postsString : postsMobileString;
			postsString = postsString.Replace("\\\"", "\"").Replace("\\\\", "\\");
			var posts = JsonConvert.DeserializeObject<PostDefinition[]>(postsString);
			foreach (var post in posts)
			{
				await downloadedPostsManager.TryDownloadPostAsync(post, client);
			}
		}

        //var images = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(memeImages);
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

        await Navigation.PushAsync(new BrowsePage(downloadedPostsManager));

        SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

