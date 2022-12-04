using Microsoft.EntityFrameworkCore;
using Offline9GagDownloader._9Gag.DB;

namespace Offline9GagDownloader._9Gag
{
    internal class PostsDbContext : DbContext
    {
        public DbSet<PostModel> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./Posts.sqlite");
        }
    }
}
