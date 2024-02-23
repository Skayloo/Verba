using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Verba.Stock.Domain.Models.Snilses;

namespace Verba.Stock.Db.Data.EntityConfigurations
{
    public class SnilsesHashEntityTypeConfiguration : IEntityTypeConfiguration<SnilsesHash>
    {
        public void Configure(EntityTypeBuilder<SnilsesHash> builder)
        {
            builder.ToTable("snilses_hash");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedDatetime).HasColumnName("created_datetime").IsRequired(false);
            builder.Property(p => p.ModifiedDatetime).HasColumnName("modified_datetime").IsRequired(false);
            builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired(false);
            builder.Property(p => p.SnilsHash).HasColumnName("snils_hash").IsRequired(false);
            builder.Property(p => p.Date).HasColumnName("date").IsRequired(false);
            builder.Property(p => p.Count).HasColumnName("count").IsRequired(false);
            builder.Property(p => p.Country).HasColumnName("country").IsRequired(false);
        }
    }
}
