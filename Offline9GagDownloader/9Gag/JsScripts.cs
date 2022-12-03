namespace Offline9GagDownloader._9Gag
{
    internal class JsScripts
    {
        //TODO refactor as create function for each action in separate execute
//        @"posts = document.getElementsByTagName('article');
//postsarray = Array.prototype.slice.call(posts);
//postsarray.map(p => {
//	header = p.getElementsByTagName('h1')[0]?.innerText
//	let img = p.getElementsByClassName('post-container')[0]?.getElementsByTagName('img')
//	let video = p.getElementsByClassName('post-container')[0]?.getElementsByTagName('source')
//	img = img ? Array.prototype.slice.call(img) : undefined
//	video = video ? Array.prototype.slice.call(video) : undefined
//	img = img?.length ? img : video
//	img = img ? img[0]?.src : undefined
//	return {header:header, img:img};
//}).filter(post => post.header && post.img)"
        /// <summary>
        /// Unfortunately only one line scripts can be injected and evaluated
        /// </summary>
        public const string GetPosts = @"posts = document.getElementsByTagName('article'); postsarray = Array.prototype.slice.call(posts); postsarray.map(p => {	header = p.getElementsByTagName('h1')[0]?.innerText; img = p.getElementsByClassName('post-container')[0]?.getElementsByTagName('img'); video = p.getElementsByClassName('post-container')[0]?.getElementsByTagName('source'); img = img ? Array.prototype.slice.call(img) : undefined; video = video ? Array.prototype.slice.call(video) : undefined; img = img?.length ? img : video;	img = img ? img[0]?.src : undefined;	return {header:header, img:img};}).filter(post => post.header && post.img);";
    }
}
