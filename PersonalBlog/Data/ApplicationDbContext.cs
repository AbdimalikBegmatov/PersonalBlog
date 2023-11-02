using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;

namespace PersonalBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Membership> Memberships { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Membership>().Property(m => m.CreatedDate).HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(builder);
            
        }
    }
}
