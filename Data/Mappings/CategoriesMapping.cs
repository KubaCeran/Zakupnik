using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zakupnik.Data.Entities;

namespace Zakupnik.Data.Mappings
{
    public class CategoriesMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.CategoryId);
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.CategoryName).IsRequired();
        }
    }
}
