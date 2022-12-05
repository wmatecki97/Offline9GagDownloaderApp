//namespace Offline9GagDownloader;

//public partial class BrowsePage : ContentPage
//{
//	public BrowsePage()
//	{
//		InitializeComponent();
//	}
//    async void OnShowVideoLibraryClicked(object sender, EventArgs e)
//    {
//        Button button = sender as Button;
//        button.IsEnabled = false;

//        var pickedVideo = await MediaPicker.PickVideoAsync();
//        if (!string.IsNullOrWhiteSpace(pickedVideo?.FileName))
//        {
//            video.Source = new VideoPlayback.Controls.FileVideoSource
//            {
//                File = pickedVideo.FullPath
//            };
//        }

//        button.IsEnabled = true;
//    }

//    void OnContentPageUnloaded(object sender, EventArgs e)
//    {
//        video.Handler?.DisconnectHandler();
//    }
//}