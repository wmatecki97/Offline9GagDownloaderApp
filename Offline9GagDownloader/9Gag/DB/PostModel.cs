using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Offline9GagDownloader._9Gag.DB
{
    public class PostModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MediaPath { get; set; }
        public string Title { get; set; }
        public string SrcUrl { get; set; }
        public bool Displayed { get; set; }

        public PostModel()
        {
        }

        public PostModel(PostDefinition p, string mediaPath)        
        {
            Title = p.Header;
            SrcUrl = p.ImgSrc;
            CreatedDate = DateTime.Now;
            MediaPath = mediaPath;
        }


    }
}
