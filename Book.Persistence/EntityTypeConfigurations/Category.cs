using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.Persistence.EntityTypeConfigurations;

public class Category:IEntityTypeConfiguration<Models.Category>
{
    public void Configure(EntityTypeBuilder<Models.Category> builder)
    {
        builder.HasKey(c => c.CategoryName);
        builder.HasMany(c => c.BookPosts)
            .WithOne(b => b.Category)
            .HasForeignKey("CategoryName").IsRequired();
    }
}