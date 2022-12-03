namespace Offline9GagDownloader._9Gag
{
	internal class JsScripts
    {
		public const string GetPosts = @"
{
posts = document.getElementsByTagName('article')
postsarray = Array.prototype.slice.call(posts)
return postsarray.map(p => {
	header = p.getElementsByTagName('h1')[0]?.innerText
	let img = p.getElementsByClassName('post-container')[0]?.getElementsByTagName('img')
	let video = p.getElementsByClassName('post-container')[0]?.getElementsByTagName('source')
	imgs = imgs ? Array.prototype.slice.call(imgs) : undefined
	video = video ? Array.prototype.slice.call(video) : undefined
	img = img?.length ? img : video
	img = img ? img[0]?.src : undefined
	return {header:header, img:img};
}).filter(post => post.header && post.img)
}";
    }
}
