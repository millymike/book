using Book.Models;
using Microsoft.EntityFrameworkCore;

namespace Book.Persistence;

public class DataContext : DbContext
{
    
     public DataContext()
    {
    }
    
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<BookPost?> BookPosts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User?> Users { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.User());
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.BookPost());
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.Category());
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.Author());
    }
}