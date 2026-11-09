using Microsoft.EntityFrameworkCore;

namespace APP.Domain
{
    public class Db : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<BlogTag> BlogTags { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }
    }
}