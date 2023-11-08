using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;

namespace PersonalBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publication> Publications { get; set; }    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Membership>().Property(m => m.CreatedDate).HasDefaultValueSql("GETDATE()");
            builder.Entity<Publication>()
                .HasMany<Category>(p => p.Categories)
                .WithMany(c=>c.Publications)
                .UsingEntity(p=>p.ToTable("PublicationCategoryRelations"));

            builder.Entity<Publication>().Property(p=>p.TotalViews).HasDefaultValue(1);
            builder.Entity<Publication>().Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(builder);
            
        }
    }
}
