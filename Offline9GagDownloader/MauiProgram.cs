using Microsoft.Extensions.DependencyInjection;
using Offline9GagDownloader._9Gag;
using Offline9GagDownloader._9Gag.DB;
using VideoPlayback.Controls;
using VideoPlayback.Handlers;

namespace Offline9GagDownloader;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(Video), typeof(VideoHandler));
            });
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddScoped<IDownloadedPostsManager, DownloadedPostsManager>();
		//builder.Services.AddHttpClient();
		builder.Services.AddSingleton<IPostDatabase, PostDatabase>();
        Routing.RegisterRoute("home", typeof(MainPage));
        return builder.Build();
    }
}
