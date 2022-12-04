using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Offline9GagDownloader._9Gag.DB
{
    internal class PostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; }
        public string MediaPath { get; }
        public string Title { get; }
        public string SrcUrl { get; }

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
