using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.Persistence.EntityTypeConfigurations;

public class Author:IEntityTypeConfiguration<Models.Author>
{
    public void Configure(EntityTypeBuilder<Models.Author> builder)
    {
        builder.HasKey(a => a.AuthorId);
        builder.Property(a => a.Name).IsRequired();
        builder.HasMany(a => a.BookPosts)
            .WithOne(b => b.Author).IsRequired();
    }
}