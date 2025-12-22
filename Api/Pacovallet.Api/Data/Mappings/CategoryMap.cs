using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pacovallet.Api.Models;

namespace Pacovallet.Api.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    { 
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Tabela
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Purpose)
                .IsRequired()
                .HasConversion<int>();

            builder.HasIndex(c => c.UserId);            
        }
    }
}
