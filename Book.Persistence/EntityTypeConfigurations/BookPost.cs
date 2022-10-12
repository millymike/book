using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.Persistence.EntityTypeConfigurations;

public class BookPost:IEntityTypeConfiguration<Models.BookPost>
{
    public void Configure(EntityTypeBuilder<Models.BookPost> builder)
    {
        builder.HasKey(b => b.BookId);
        builder.Property(b => b.Body).IsRequired();
        builder.Property(b => b.Title).IsRequired();
        builder.Property(b => b.Tags).IsRequired();
        builder.HasOne(b => b.Author)
            .WithMany(a => a.BookPosts);
        builder.HasOne(b => b.Category)
            .WithMany(c => c.BookPosts);
    }
}