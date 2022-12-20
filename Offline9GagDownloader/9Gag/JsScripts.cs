namespace Offline9GagDownloader._9Gag
{
    internal class JsScripts
    {
        /// <summary>
        /// Unfortunately only one line scripts can be injected and evaluated
        /// </summary>
        public const string GetPosts =      @"posts = document.getElementsByTagName('article'); postsarray = Array.prototype.slice.call(posts); postsarray.map(p => { header = p.querySelectorAll('h1, h2, h3', 'h4', 'h5')[0]?.innerText.replaceAll('""', '\''); img = p.getElementsByClassName('post-container')[0]?.getElementsByTagName('img'); video = p.getElementsByClassName('post-container')[0]?.getElementsByTagName('source'); img = img ? Array.prototype.slice.call(img) : undefined; video = video ? Array.prototype.slice.call(video) : undefined; img = img?.length ? img : video;	img = img ? img[0]?.src : undefined;	return {header:header, imgSrc:img};}).filter(post => post.header && post.imgSrc);";
        public const string GetPostsMobile = "posts = document.getElementsByTagName('article'); postsarray = Array.prototype.slice.call(posts); postsarray.map(p => { header = p.querySelectorAll('h1, h2, h3', 'h4', 'h5')[0]?.innerText.replaceAll('\"', '\\''); let            img = p.getElementsByClassName('post-content')[0]?.getElementsByTagName('img'); let video = p.getElementsByClassName('post-content')[0]?.getElementsByTagName('source'); img = img ? Array.prototype.slice.call(img) : undefined; video = video ? Array.prototype.slice.call(video) : undefined; img = img?.length ? img : video; img = img ? img[0]?.src : undefined; return {header:header, imgSrc:img};}).filter(post => post.header && post.imgSrc)";
    }
}
