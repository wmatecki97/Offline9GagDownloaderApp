﻿using Microsoft.Extensions.DependencyInjection;
using Offline9GagDownloader._9Gag;

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
			});
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddScoped<IDownloadedPostsManager, DownloadedPostsManager>();
		builder.Services.AddHttpClient();
		builder.Services.AddEntityFrameworkSqlite().AddDbContext<PostsDbContext>();
		return builder.Build();
	}
}
